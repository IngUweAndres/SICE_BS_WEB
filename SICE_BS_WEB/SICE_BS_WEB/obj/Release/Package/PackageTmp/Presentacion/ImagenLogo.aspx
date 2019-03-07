<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="ImagenLogo.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.ImagenLogo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //Inicializa todos los tooltip
        $(function () {
            $('[data-toggle="popover"]').popover();
            $('[data-toggle="tooltip"]').tooltip({
                delay: 500,
                trigger: 'hover'
            });
        });

        var CustomPager = {
            gotoBox_Init: function (s, e) {
                s.SetText(1 + grid.GetPageIndex());
            },
            gotoBox_KeyPress: function (s, e) {
                if (e.htmlEvent.keyCode != 13)
                    return;
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                CustomPager.applyGotoBoxValue(s);
            },
            gotoBox_ValueChanged: function (s, e) {
                CustomPager.applyGotoBoxValue(s);
            },
            applyGotoBoxValue: function (textBox) {
                if (grid.InCallback())
                    return;
                var pageIndex = parseInt(textBox.GetText()) - 1;
                if (pageIndex < 0)
                    pageIndex = 0;
                grid.GotoPage(pageIndex);
            },
            combo_SelectedIndexChanged: function (s, e) {
                grid.PerformCallback(s.GetSelectedItem().text);
            }
        };

        function OnToolbarItemClick(s, e) {
            if (IsExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                e.usePostBack = true;
            }
        }
        function IsExportToolbarCommand(command) {
            return command == "ExportToPDF" || command == "ExportToXLSX" || command == "ExportToXLS";
        }

        function ColorChangedHandler(s, e) {
            CallbackPanel.PerformCallback();
        }

        //Función llamada por método chkConsultarIzq_Init()
        function chkConsultarIzqClick(s, e, index) {
            var cheked = s.GetChecked();

            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
                var chkConsultarIzq = eval("chkConsultarIzq" + i);
                chkConsultarIzq.SetChecked(false);

                if (i == index)
                    chkConsultarIzq.SetChecked(cheked);
            }
        }

        //Función llamada por método chkConsultarDer_Init()
        function chkConsultarDerClick(s, e, index) {
            var cheked = s.GetChecked();

            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
                var chkConsultarDer = eval("chkConsultarDer" + i);
                chkConsultarDer.SetChecked(false);

                if (i == index)
                    chkConsultarDer.SetChecked(cheked);
            }
        }

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
                      
    <div class="container-fluid">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
            </div>
            <div class="panel-body" align="left">
                <asp:Panel ID="Panel1" runat="server">
                    <div class="row" style="padding-left: 10px; padding-top: 10px; padding-right: 10px;">
                        <div class="col-md-8">
                            <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" SettingsPager-Mode="ShowAllRecords"                           
                                    EnableCallBacks="false" ClientInstanceName="grid" AutoGenerateColumns="False" Width="776px"
                                    OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback" KeyFieldName="IdLogo" 
                                    Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px"  >
                                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800"
                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                        <AdaptiveDetailLayoutProperties ColCount="2">
                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                        </AdaptiveDetailLayoutProperties>
                                    </SettingsAdaptivity>
                                    <SettingsBehavior AllowSelectByRowClick="false" />
                                    <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false" />
                                    <SettingsPopup>
                                        <HeaderFilter Height="200px" Width="195px" />
                                    </SettingsPopup>
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="IdLogo" ReadOnly="True" VisibleIndex="0" Visible="false">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataColumn Caption="Logo</br>izquierdo" VisibleIndex="0" Width="80px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>                                    
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkConsultarIzq" ClientInstanceName="chkConsultarIzq" Checked='<%# Eval("ValidaIzquierdo") %>' Enabled="true" runat="server" OnInit="chkConsultarIzq_Init">
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Logo</br>derecho" VisibleIndex="0" Width="80px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>                                    
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkConsultarDer" ClientInstanceName="chkConsultarDer" Checked='<%# Eval("ValidaDerecho") %>' Enabled="true" runat="server" OnInit="chkConsultarDer_Init">
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewCommandColumn Caption="" VisibleIndex="1" Width="80px">
                                            <CustomButtons>                                                                            
                                                <dx:GridViewCommandColumnCustomButton ID="btnEditar" Text="Editar" 
                                                    Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                    Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                     
                                                    <Styles>
                                                        <Style Font-Bold="False" Font-Size="11px">
                                                            <HoverStyle Font-Bold="False">
                                                            </HoverStyle>
                                                        </Style>
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewCommandColumn Caption="" VisibleIndex="2" Width="80px">
                                            <CustomButtons>                                                                            
                                                <dx:GridViewCommandColumnCustomButton ID="btnEliminar" Text="Eliminar" 
                                                    Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                    Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                     
                                                    <Styles>
                                                        <Style Font-Bold="False" Font-Size="11px">
                                                            <HoverStyle Font-Bold="False">
                                                            </HoverStyle>
                                                        </Style>
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="Nombre del Archivo" FieldName="FileName" ReadOnly="True" VisibleIndex="3" Width="210px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <SettingsHeaderFilter Mode="CheckedList" />
                                            <CellStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataBinaryImageColumn Caption="Logo" FieldName="Logo" VisibleIndex="4" Width="256px" PropertiesBinaryImage-ImageHeight="97px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <PropertiesBinaryImage ImageHeight="150" ImageWidth="225" EnableServerResize="True">
                                                <EditingSettings Enabled="false" />
                                            </PropertiesBinaryImage>
                                        </dx:GridViewDataBinaryImageColumn>
                                    </Columns>
                                    <Toolbars>
                                    <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                        <Items>
                                            <dx:GridViewToolbarItem Name="Links">
                                            <Template>
                                                <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_Nuevo_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Nuevo Logo" data-toggle="tooltip">
                                                    <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lkb_Aceptar" runat="server" OnClick="lkb_Aceptar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Aceptar Logo" data-toggle="tooltip">
                                                    <span class="glyphicon glyphicon-log-in"></span>&nbsp;&nbsp;Aceptar
                                                </asp:LinkButton>                                                             
                                            </Template>
                                           </dx:GridViewToolbarItem>           
                                        </Items>
                                    </dx:GridViewToolbar>                                               
                                    </Toolbars>                                                  
                                </dx:ASPxGridView>
                            <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                                <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                            </dx:ASPxCallback>
                            <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                                Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                            </dx:ASPxLoadingPanel>
                            <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                        </div>
                        <div class="col-md-4">
                            <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false" Width="100%">
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                <Items>                                        
                                    <dx:LayoutGroup Caption="Información Importante" ColCount="1" SettingsItemCaptions-Location="Top">
                                        <Items>
                                            <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                        <asp:Panel runat="server" HorizontalAlign="Justify">
                                                            <p><asp:Label Text="* Si después de guardar no ve los cambios reflejados, ir a:" Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- IE</b>: Seleccionar Opciones de Internet, después seleccionar la pestaña General y dar clic en botón de Eliminar, seleccionae la casilla de Archivos temporales de Internet y archivos de sitio web y seleccionar botón Eliminar" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- Mozilla FireFox</b>: Seleccionar Opciones, después seleccionar del menú izquierdo la opción de Privacidad y Seguridad, ir a la parte donde dice Cookies y datos del sitio, dar clic en botón Limpiar datos..., dentro seleccionar la casilla Contenido web en caché y dar clic en botón Limpiar" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- Google Chrome</b>: Seleccionar Más herramientas, después seleccionar Borrar datos de navegación, en la pestaña Configuración avanzada seleccionar la casilla de Archivos e imágenes almacenados en caché, después dar clic en botón Borrar los datos" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                        </asp:Panel>                                                        
                                                        </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                        </Items>
                                    </dx:LayoutGroup>
                                    <%--<dx:EmptyLayoutItem />  --%>                                                                                                            
                                </Items>
                            </dx:ASPxFormLayout>
                        </div>
                        
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>


 <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
 <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" runat="server"></h4>
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12" align="center">                                
                                <dx:ASPxBinaryImage ID="BinaryImage" ClientInstanceName="ClientBinaryImage" Width="200" Height="200"  
                                    ShowLoadingImage="true" LoadingImageUrl="~/Content/Loading.gif" runat="server">
                                    <EditingSettings Enabled="true">
                                        <UploadSettings >
                                            <UploadValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".bmp"></UploadValidationSettings>
                                        </UploadSettings>
                                    </EditingSettings>
                                </dx:ASPxBinaryImage>
                                <br />
                                <br />
                                <asp:TextBox ID="txt_FileName" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Nombre Archivo"></asp:TextBox>    
                            </div>                            
                        </div>                            
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
 <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
 <div class="modal fade" id="AlertError" tabindex="-1" role="dialog">
     <div class="modal-dialog modal-sm" role="document" style="top: 25%; outline: none;">
         <div class="alert alert-danger text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
             <span class="glyphicon glyphicon-alert ico"></span>
             <br />
             <br />
             <p id="pModal" runat="server" class="alert-title">
             </p>                                
             <hr/>
             <button type="button" class="btn btn-danger" data-dismiss="modal">Aceptar</button>
         </div>
     </div>
 </div>

 <button id="btnSuccess" type="button" data-toggle="modal" data-target="#AlertSuccess" style="display: none;"></button>
 <div class="modal fade" id="AlertSuccess" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
     <div class="modal-dialog modal-sm" role="document">
         <div class="alert alert-success text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
             <span class="glyphicon glyphicon-thumbs-up ico"></span>
             <br />
             <br />
             <p id="pModalSucces" runat="server" class="alert-title">
             </p>
             <hr />
             <button type="button" class="btn btn-success" data-dismiss="modal">Aceptar</button>
         </div>
     </div>
 </div>
   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>