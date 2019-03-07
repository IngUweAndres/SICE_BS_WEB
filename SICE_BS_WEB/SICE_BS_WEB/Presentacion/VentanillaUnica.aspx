<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="VentanillaUnica.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.VentanillaUnica" %>

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


        /* función que se usa en el Uploaded   */
        function onFilesUploadStart(s, e) {

        }

        /* función que se usa en el Uploaded fkeyfile */
        function OnUploadComplete_fk(arg) {
            lblFiles_fk.SetText(arg.callbackData);
            document.getElementById('<%=btnQuitar_fk.ClientID%>').style.display = "block";
            document.cookie = "CookieValorFK=si_guarda;";
        }

        function QuitarFile_fk() {
            lblFiles_fk.SetText('');
            document.getElementById('<%=btnQuitar_fk.ClientID%>').style.display = "none";
            document.cookie = "CookieValorFK=no_guarda;";
        }


        /* función que se usa en el Uploaded fcfile  */
        function OnUploadComplete_fc(arg) {
            lblFiles_fc.SetText(arg.callbackData);
            document.getElementById('<%=btnQuitar_fc.ClientID%>').style.display = "block";

            //Se usa para validar si se guarda o no
            document.cookie = "CookieValorFC=si_guarda;";
        }

        function QuitarFile_fc() {
            lblFiles_fc.SetText('');
            document.getElementById('<%=btnQuitar_fc.ClientID%>').style.display = "none";

            //Se usa para validar si se guarda o no
            document.cookie = "CookieValorFC=no_guarda;";
        }
             
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">    
            
    <div class="container-fluid">
         <div class="panel panel-primary">
             <div class="panel-heading">
                 <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
             </div>
             <div class="panel-body" align="center">
                 <asp:Panel ID="Panel1" runat="server">
                 <div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px;">
                     <div class="col-xs-4 col-md-1">
                         <div runat="server" id="DivEditar">
                             <div class="form-group" style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                 <div class="input-group">
                                     <dx:BootstrapButton ID="btnEdtar" runat="server" AutoPostBack="false" OnClick="btnEdtar_OnClick" Text="Editar" CssClasses-Text="txt-sm" >
                                         <CssClasses Icon="glyphicon glyphicon-pencil" />
                                         <SettingsBootstrap RenderOption="Primary" Sizing="Small" />                                                                                
                                         <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel0.Show(); }" />
                                     </dx:BootstrapButton>                                                            
                                 </div>
                             </div>
                         </div>
                     </div>
                     <div class="col-xs-12">
                        <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" 
                            OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid" AutoGenerateColumns="False" Width="100%"
                            Settings-HorizontalScrollBarMode="Auto" KeyFieldName="PEDIMENTOARMADO" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px">
                            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                <AdaptiveDetailLayoutProperties colcount="2">
                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                </AdaptiveDetailLayoutProperties>
                            </SettingsAdaptivity> 
                            <SettingsResizing ColumnResizeMode="Control" />
                            <SettingsBehavior AllowSelectByRowClick="true" />
                            <Settings ShowFooter="True" ShowHeaderFilterButton="false" ShowFilterRowMenu="false" />                                          
                            <Styles>                                                               
                                <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                <Row  />
                                <AlternatingRow Enabled="True" />
                                <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                <Footer Font-Size="11px" Font-Bold="true"/>
                            </Styles>
                            <SettingsPopup>
                                <HeaderFilter Height="200px" Width="195px"/>
                            </SettingsPopup>
                            <Columns>                        
                                <dx:GridViewDataTextColumn Caption="RFC" FieldName="RFC" ReadOnly="True" VisibleIndex="1" Width="130px" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="EMPRESA" FieldName="EMPRESA" ReadOnly="True" VisibleIndex="2" Width="260px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="PASSWORD" FieldName="PASSWORD" ReadOnly="True" VisibleIndex="3" Width="440px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="CLAVE LLAVE" FieldName="CLAVEABRIRLLAVE" ReadOnly="True" VisibleIndex="4" Width="120px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="KEYFILE" FieldName="KEYFILE" ReadOnly="True" VisibleIndex="5" Width="270px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="CERFILE" FieldName="CERFILE" ReadOnly="True" VisibleIndex="6" Width="250px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataTextColumn>                                                                        
                            </Columns>                                                                              
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="Exporter" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />                                                                                         
                        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                            <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                        </dx:ASPxCallback>
                        <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                        </dx:ASPxLoadingPanel>
                        <dx:ASPxLabel ID="ASPx_Mensaje" runat="server" Text="" Visible="false"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                     </div>
                 </div>                 
                </asp:Panel>             
             </div>             
         </div>
    </div>
    <button id="btnModalSettings" type="button" data-toggle="modal" data-target="#ModalSettings" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>        
    <div class="modal fade" id="ModalSettings" tabindex="-1" role="dialog" aria-labelledby="ModalSettingsTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-header">
                        <h6 id="ModalSettingsTitulo" class="modal-title" runat="server"></h6>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">&times;</span>
                        </button>
                    </div>                
                    <div class="modal-body">                    
                        <div class="row" style="margin-left:2px">
                            <div class="row" style="margin-right:5px;margin-bottom:5px">                                
                                <div class="col-xs-12">                           
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" title="Password" data-toggle="tooltip">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <span id="span5" runat="server" class="glyphicon glyphicon-eye-close" aria-hidden="true" ></span>
                                            </span>
                                            <asp:TextBox ID="txt_settings_password" runat="server" CssClass="form-control input-sm" MaxLength="150" TextMode="Password" placeholder="password" Font-Size="11px"></asp:TextBox>
                                        </div>
                                    </div>                            
                                </div>
                                <div class="col-xs-12">                           
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" title="Clave abrir llave" data-toggle="tooltip">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <span id="span6" runat="server" class="glyphicon glyphicon-eye-close" aria-hidden="true" ></span>
                                            </span>
                                            <asp:TextBox ID="txt_settings_claveabrirllave" runat="server" CssClass="form-control input-sm" MaxLength="50" TextMode="Password" placeholder="clave abrir llave" Font-Size="11px"></asp:TextBox>
                                        </div>
                                    </div>                            
                                </div>
                                <div class="col-xs-12" style="padding-bottom:15px">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" title="Archivo fkeyfile" data-toggle="tooltip">
                                        <dx:ASPxUploadControl ID="ucFKeyfile" runat="server" UploadMode="Auto" Width="100%" ShowProgressPanel="true" ShowUploadButton="true" FileUploadMode="BeforePageLoad" 
                                            OnFileUploadComplete="ucFKeyfile_FileUploadComplete" NullText="Arrastre un archivo fkfile aquí" AutoStartUpload="true" ShowClearFileSelectionButton="true">
                                            <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete_fk(e); }" FilesUploadStart="onFilesUploadStart" />
                                            <AdvancedModeSettings EnableMultiSelect="false" EnableFileList="false" EnableDragAndDrop="true"></AdvancedModeSettings>                                    
                                            <BrowseButton Text="Archivo"  /> 
                                        </dx:ASPxUploadControl>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="text-align:left;">
                                                    <table>
                                                        <tr>
                                                            <td style="text-align:left;">
                                                                <dx:ASPxLabel ID="lblTitRespuestaFileUpload_fk" runat="server" Font-Size="11px" ClientInstanceName="lblFiles_fk"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td style="text-align:left;">
                                                                <asp:LinkButton ID="btnQuitar_fk" runat="server"  OnClientClick="QuitarFile_fk(); return false" Font-Size="11px" 
                                                                    ForeColor="Black" CssClass="no-display" ClientIDMode="AutoID">quitar
                                                                </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>                                            
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="col-xs-12" style="padding-bottom:3px">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" title="Archivo fcerfile" data-toggle="tooltip">
                                        <dx:ASPxUploadControl ID="ucfcfile" runat="server" UploadMode="Auto" Width="100%" ShowProgressPanel="true" ShowUploadButton="true" FileUploadMode="BeforePageLoad" 
                                            OnFileUploadComplete="ucfcfile_FileUploadComplete" NullText="Arrastre un archivo fcerfile aquí" AutoStartUpload="true" ShowClearFileSelectionButton="true" >
                                            <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete_fc(e); }" FilesUploadStart="onFilesUploadStart" />
                                            <AdvancedModeSettings EnableMultiSelect="false" EnableFileList="false" EnableDragAndDrop="true"></AdvancedModeSettings>                                    
                                            <BrowseButton Text="Archivo"  /> 
                                        </dx:ASPxUploadControl>
                                        <table style="width:100%">
                                            <tr>
                                                <td style="text-align:left;">
                                                    <table>
                                                        <tr>
                                                            <td style="text-align:left;">
                                                                <dx:ASPxLabel ID="lblTitRespuestaFileUpload_fc" runat="server" Font-Size="11px" ClientInstanceName="lblFiles_fc"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td style="text-align:left;">
                                                                <asp:LinkButton ID="btnQuitar_fc" runat="server"  OnClientClick="QuitarFile_fc(); return false" Font-Size="11px" 
                                                                    ForeColor="Black" CssClass="no-display" ClientIDMode="AutoID">quitar
                                                                </asp:LinkButton>                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>                                            
                                            </tr>
                                        </table>
                                    </div>                           
                                </div>
                                <dx:ASPxLabel ID="lblkeyfile" runat="server" Visible="false"/>
                                <dx:ASPxLabel ID="lblcerfile" runat="server" Visible="false"/>
                                <%--<div class="col-xs-12 col-sm-12 col-md-4" style="padding-bottom:3px">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnGuardar_Click" ToolTip="Guardar Archivo">
                                     <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar Archivo
                                    </asp:LinkButton>
                                </div>--%>    
                            </div>
                        </div>
                    </div>                
                    <div class="modal-footer">                        
                         <%--<div class="col-xs-12 col-sm-4 col-md-4 text-center">
                             <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnGuardar_Click" ToolTip="Guardar Archivo(s)">
                                 <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar Archivo(s)</asp:LinkButton>                          
                         </div>--%>  
                        <div class="col-xs-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarSettings" runat="server" CssClass="btn btn-primary btn-sm" ToolTip="" OnClick="btnGuardarSettings_Click">
                                 <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <asp:LinkButton ID="btncerrar" runat="server" CssClass="btn btn-primary btn-sm" ToolTip="" OnClick="btncerrar_Click">
                                 <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                            <%--<button id="btncerrar" runat="server" type="button" class="btn btn-primary btn-sm" data-dismiss="modal" onclick="">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>--%>
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
