<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FormaPago.aspx.cs" Inherits="SAT.FacturacionElectronica33.FormaPago" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Estilos de los controles-->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet"/>
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet"/>
    <!--Estilos de AutoComplete, Máscara y Validadores JQuery-->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet"/>
    <!--Idioma de la validacion-->
    <script type ="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset ="utf-8"></script>
    <!--Validation Engine-->
    <script type ="text/javascript" src="../Scripts/jquery.validationEngine.js" charset ="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Valida las inserciones de datos en los controles-->
    <script type="text/javascript">
        //Obteniendo instancia actual de la página añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Crear la funcion que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no está definido
            if (args.get_error() == undefined) {
                //Invoca a la funcion ConfiguraFormaPago
                ConfiguraFormaPago();
            }
        }
        //Declarando función de configuracion para validar los controles de la página
        function ConfiguraFormaPago() {
            $(document).ready(function () {
                var validaFormaPago = function () {
                    //Crear las variables y asignar los controles de la página
                    var isValid1 = !$("#<%=txtClave.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtCuentaOrdenante.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtCuentaBeneficiario.ClientID%>").validationEngine('validate');
                    //Devuelve el valor de la función
                    return isValid1 && isValid && isValid3 && isValid4;
                };
                //Controles que ejecutan las validaciones
                $("#<%=btnAceptar.ClientID%>").click(validaFormaPago);
                $("#<%=lnkGuardar.ClientID%>").click(validaFormaPago);            
            });            
        }
        //Llamar a la configuracion
        ConfiguraFormaPago();
    </script>
    <!--Fin del Script de validación-->
    <div id="encabezado_forma">
        <img src="../Image/Billete.png" width ="4%"/>
        <h1>  Forma de Pago</h1>
    </div>
    <!--Inicio Barra de Navegacion-->
    <nav id="menuForma">
        <ul>
            <li class="green"><a href="#" class="fa fa-floppy-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplnkNuevo" runat="server" UpdateMode ="Conditional" >
                            <ContentTemplate >
                                <asp:LinkButton ID="lnkNuevo" runat="server" Text="Nuevo" OnClick="lnkElementoMenu_Click" CommandName="Nuevo" />
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>                                
                            </Triggers>
                        </asp:UpdatePanel>                        
                    </li>                    
                    <li>
                        <asp:UpdatePanel ID="uplnkAbrir" runat ="server" UpdateMode ="Conditional">
                            <ContentTemplate >
                                <asp:LinkButton ID="lnkAbrir" runat="server" Text="Abrir" OnClick="lnkElementoMenu_Click" CommandName="Abrir" />
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplnkGuardar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkGuardar" runat="server" Text="Guardar" OnClick="lnkElementoMenu_Click" CommandName="Guardar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="red"><a href="#" class="fa fa-pencil-square-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID ="uplnkEditar" runat ="server" UpdateMode ="Conditional" >
                            <ContentTemplate >
                                <asp:LinkButton ID="lnkEditar" runat="server" Text ="Editar" OnClick ="lnkElementoMenu_Click" CommandName="Editar"/>
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>  
                            </Triggers>
                        </asp:UpdatePanel>  
                    </li>

                    <li>
                        <asp:UpdatePanel ID="uplnkEliminar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkElementoMenu_Click" CommandName="Eliminar" />
                            </ContentTemplate>
                            <Triggers>                            
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>
                                
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="blue"><a href="#" class="fa fa-cog"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplbkBitacora" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkBitacora" runat="server" Text="Bitácora" OnClick="lnkElementoMenu_Click" CommandName="Bitacora" Enabled ="false" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>
                                
                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplnkReferencia" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkReferencias" runat="server" Text="Referencias" OnClick="lnkElementoMenu_Click" CommandName="Referencias" Enabled ="false" />
                            </ContentTemplate>
                            <Triggers>                            
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>

                                <asp:PostBackTrigger ControlID ="lnkAbrir"/>

                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>            
        </ul>
    </nav>
    <!--Panel Izquierdo-->
    <asp:Panel runat ="server" DefaultButton ="btnAceptar" ID ="pnlPanelIzquierdo">
        <!--Seccion Controles-->
        <div class="seccion_controles" style="width:40%">
                <!--Encbezado-->
                <div class="header_seccion">
                    <img src="../Image/Registro.png" width ="5%"/>
                    <h2>Detalles de la Forma de Pago</h2>
                </div>
                <!--Columna-->
                <div class="columna2x">
                    <div class="renglon2x"></div><!--Renglón vacío para dar espacio-->
                    <!--Renglón Clave-->
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label class="label" for ="txtClave">Clave:</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtClave" runat ="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox CssClass="textbox_30px validate[required,custom[integer]]" ID="txtClave" runat="server" TabIndex="1" MaxLength="2"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Renglón Descripcion-->
                    <div class="renglon2x" style ="height:40px">
                        <div class="etiqueta">
                            <label class="Label" for ="txtDescripcion">Descripción:</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtDescripcion" runat ="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox CssClass="textbox2x validate[required]" ID="txtDescripcion" runat="server" TabIndex="2" TextMode="MultiLine" MaxLength="50"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Renglon Label Cuentas-->
                    <div class="renglon2x">
                        <div class="etiqueta" style ="width:300px; align-content:center">
                            <label class="label_negrita" style ="width:100px">PATRÓN DE CUENTA</label>
                        </div>
                    </div>
                    <!--Renglón Patron Cuenta Ordenante-->
                    <div class="renglon2x" style="height:40px">
                        <div class="etiqueta">
                            <label class="Label" for ="txtCuentaOrdenante">Ordenante:</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtCuentaOrdenante" runat ="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox CssClass="textbox2x validate[required]" ID="txtCuentaOrdenante" runat="server" TabIndex="3" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Renglón Patron Cuenta Beneficiario-->
                    <div class="renglon2x" style ="height:40px">
                        <div class="etiqueta">
                            <label class="Label" for ="txtCuentaBeneficiario">Beneficiario:</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtCuentaBeneficiario" runat ="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox CssClass="textbox2x validate[required]" ID="txtCuentaBeneficiario" runat="server" TabIndex="4" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Renglón vacío para dar espacio-->
                    <div class="renglon2x"></div>            
                    <!--Renglón botones-->
                    <div class="renglon_boton">              
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelar" runat ="server" UpdateMode ="Conditional" >
                                <ContentTemplate >
                                    <asp:Button runat="server" ID="btnCancelar" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="6"/>                    
                                </ContentTemplate>
                                <Triggers >
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel> 
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnAceptar" runat ="server" UpdateMode ="Conditional" >
                                <ContentTemplate >
                                    <asp:Button runat="server" ID="btnAceptar" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" TabIndex="5"/>
                                </ContentTemplate>
                                <Triggers >
                                    <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                    <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                </Triggers>
                            </asp:UpdatePanel>               
                        </div>
                    </div>
                </div>        
            </div>
    </asp:Panel>
    <!--Panel Derecho-->
    <asp:Panel runat="server" DefaultButton ="btnAgregarValidacion" ID="pnlPanelDerecho">
        <!--Seccion GridView-->
        <div class="seccion_controles" style ="width:50%">
            <!--Encabezado-->
            <div class="header_seccion">
            <img src="../Image/Registro.png" width="4%" />
            <h2>Validaciones</h2>
        </div>
            <div class="columna" style="width:99%">
                <!--GridView-->
                <div class="contenedor_730px_derecha" style="width:inherit">
                    <div class="renglon3x">
                        <!--Label Agregar Validacion-->
                        <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uplblAccionValidacion" runat ="server" UpdateMode="Conditional">
                            <ContentTemplate >
                                <asp:Label ID="lblAccionValicacion" runat="server">Agregar:</asp:Label>
                            
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="gvValidacionFormaPago" />
                                <asp:AsyncPostBackTrigger ControlID ="btnAgregarValidacion" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    
                    </div>
                        <!--DropDownList validaciones-->
                        <div class="control">
                        <asp:UpdatePanel ID="updllValidaciones" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlValidaciones" runat="server" AutoPostBack ="true" Class="dropdown" TabIndex ="7"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="gvValidacionFormaPago" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                        <!--Botón Agregar Validacion-->
                        <div class="control" style ="width:auto">
                        <asp:UpdatePanel ID="upbtnAgregarValidacion" runat ="server" UpdateMode ="Conditional" >
                            <ContentTemplate >
                                <asp:Button runat="server" ID="btnAgregarValidacion" CssClass="boton" Text="Agregar" OnClick="btnAgregarValidacion_Click" Width="60px" Height="24px" TabIndex ="8"/>
                            </ContentTemplate>
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID ="btnAceptar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkGuardar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkNuevo" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkEditar" EventName ="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="lnkEliminar" EventName ="Click" />
                            </Triggers>
                        </asp:UpdatePanel>               
                    </div>
                        <!--Label Ordenado Por:-->
                        <div class ="etiqueta" style="width:auto">
                        <label for="lblOrdenado">Ordenado: </label>
                    </div>
                        <!--Label que muestra el orden-->
                        <div class="etiqueta" style="width:130px">
                        <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode ="Conditional">
                            <ContentTemplate>
                                <b><asp:Label ID="lblOrdenado" runat="server" Width="120px"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvValidacionFormaPago" EventName="Sorting"/>
                            </Triggers>
                        </asp:UpdatePanel> 
                    </div>
                        <!--Link Exportar-->
                        <div class="etiqueta_50pxr" >
                        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" CommandName = "ValidacionFormaPago" OnClick="lnkExportar_Click" TabIndex ="9"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportar" />
                            </Triggers>
                        </asp:UpdatePanel>                    
                    </div>
                    </div>            
                    <!--GridView/Tabla-->
                    <div class="grid_seccion_completa_200px_altura">
                        <asp:UpdatePanel ID="upgvValidacionFormaPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:GridView ID="gvValidacionFormaPago" runat="server" AllowSorting ="true" PageSize="8"
                            CssClass="gridview" ShowFooter="true" OnSorting="gvValidacionFormaPago_Sorting"
                            AutoGenerateColumns="false" Width ="100%">
                                <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter"/>
                                <HeaderStyle CssClass="gridviewheader"/>
                                <RowStyle CssClass="gridviewrow"/>
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false"/>
                                    <asp:BoundField DataField="DescripcionValidacion" HeaderText="Validación" SortExpression="DescripcionValidacion"/>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" CommandName="Editar" OnClick="lnkActualizaValidacion_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" OnClick="lnkActualizaValidacion_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lnkGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lnkEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lnkEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlValidaciones" />
                            <asp:AsyncPostBackTrigger ControlID ="btnAgregarValidacion" />
                        </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>