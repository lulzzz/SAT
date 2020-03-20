<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="SerieFolioCFDI.aspx.cs" Inherits="SAT.FacturacionElectronica33.SerieFolioCFDI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos de controles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Idioma de validación -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <!-- ValidationEngine -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Valida las inserciones de datos en los controles-->
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Crear la funcion que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no está definido
            if (args.get_error() == undefined) {
                //Invoca a la funcion ConfiguraSerieFolioCFDI
                ConfiguraSerieFolioCFDI();
            } 
        }
        //Declarando Función de Configuración para validar los controles de la página
        function ConfiguraSerieFolioCFDI() {
            $(document).ready(function () {
                //Crear y asignar la funcion a la variable ValidaSerieFolioCFDI
                var validaSerieFolioCFDI = function () {
                    //Crear las variables y asignar los controles de la página
                    var isValid1 = !$("#<%=txtCompaniaEmisor.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtVersion.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtFolioInicial.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtFolioFinal.ClientID%>").validationEngine('validate');
                    //Devuelve el valor de la funcion
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                };
                //Permite que los eventos los metodos "Aceptar" activen la funcion de validaciond e controles
                $("#<%=btnAceptar.ClientID%>").click(validaSerieFolioCFDI);
                $("#<%=lnkGuardar.ClientID%>").click(validaSerieFolioCFDI);
            });
        }
        //Invocando Función de Configuración
        ConfiguraSerieFolioCFDI();
    </script>
    <!--Fin del script-->
    <div id="encabezado_forma">
        <h1>Serie Folio CFDI</h1>
    </div>    
    <!--Menú Principal-->
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
    <!--Fin menú principal-->
    <div class="seccion_controles">
        <div class="header_seccion">
            <h2>Datos del registro</h2>
        </div>
        <div class ="columna">
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label for="lblIdSerieFolioCFDI" class="label">Registro No.:</label>
                </div>
                <div class="etiqueta_200px">
                    <asp:UpdatePanel runat="server" ID="uplblIdSerieFolioCFDI" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblIdSerieFolioCFDI" CssClass="label_negrita"></asp:Label>
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
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label class="Label" for="txtCompaniaEmisor">Compañia: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompaniaEmisor" runat="server" UpdateMode ="Conditional" >
                        <ContentTemplate>
                            <asp:TextBox CssClass="textbox" ID="txtCompaniaEmisor" runat="server" TabIndex ="1"></asp:TextBox>
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
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label class="Label" for="txtVersion">Versión:</label>
                </div>
                <div class="control_60px">
                    <asp:UpdatePanel ID="uptxtVersion" runat="server">
                        <ContentTemplate>
                            <asp:TextBox CssClass="textbox_50px validate[required]" ID="txtVersion" runat="server" MaxLength="5" TabIndex ="2"></asp:TextBox>
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
                <div class="etiqueta_50px">
                    <label class="Label" for="txtSerie">Serie:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode ="Conditional"  >
                        <ContentTemplate>
                            <asp:TextBox CssClass="textbox_100px validate[required]"   ID="txtSerie" runat="server" MaxLength="10" TabIndex ="3"></asp:TextBox>
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
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label class="label" for="ddlTipoFolioSerie">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID ="upddlTipoFolioSerie" runat ="server" UpdateMode ="Conditional" >
                        <ContentTemplate >
                            <asp:DropDownList CssClass ="dropdown" ID="ddlTipoFolioSerie" runat ="server" TabIndex ="4"></asp:DropDownList>
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
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label_negrita">FOLIO</label>
                </div>
            </div> 
            <div class="renglon3x">
                <div class="etiqueta_80px">
                    <label class="label" for="txtFolioInicial">Empieza en: </label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFolioInicial" runat="server" UpdateMode ="Conditional" >
                        <ContentTemplate >
                            <asp:TextBox CssClass="textbox_100px validate[required,custom[integer]]"  ID="txtFolioInicial" runat="server" MaxLength="10" TabIndex ="5"/>
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
                <div class="etiqueta_80px">
                    <label class="Label" for="txtFolioFinal">Termina en: </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFolioFinal" runat ="server" UpdateMode ="Conditional" >
                        <ContentTemplate >
                            <asp:TextBox CssClass="textbox_100px validate[required,custom[integer]]"   ID="txtFolioFinal" runat="server" MaxLength="10" TabIndex ="6"/>
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
            <div class="renglon">
                <div class="etiqueta_80px">
                    <label class="label" for="chkActiva">¿Activo?</label>
                </div>                
                <div class="control_60px">
                    <asp:UpdatePanel ID="upchkActiva" runat="server">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkActiva" runat="server" Text="Sí" TabIndex="7" AutoPostBack="true"/>
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
            <div class="renglon_boton">              
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat ="server" UpdateMode ="Conditional" >
                    <ContentTemplate >
                        <asp:Button runat="server" ID="btnCancelar" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="9" ValidationGroup="Contrasena" />                    
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
                        <asp:Button runat="server" ID="btnAceptar" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" TabIndex="8"/>
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
</asp:Content>