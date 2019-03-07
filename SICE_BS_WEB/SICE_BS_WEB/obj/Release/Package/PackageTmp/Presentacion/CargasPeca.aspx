<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="CargasPeca.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.CargasPeca" %>

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

        //function gridView_EndCallback(s, e) {
        //    if (s.cpResult) {
        //        window.open(s.cpResult, '_blank')
        //        delete (s.cpResult);
        //    }
        //}

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">

 <div class="container-fluid">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
        </div>
        <div class="panel-body" >
            <asp:Panel ID="Panel1" runat="server">
    
                   <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>

                   <br />
                   <dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete" ShowProgressPanel="True" ShowUploadButton="True" UploadMode="Auto" Width="320px" >
                       <AdvancedModeSettings EnableFileList="True" EnableMultiSelect="True"></AdvancedModeSettings>
                       <ValidationSettings AllowedFileExtensions=".txt"></ValidationSettings>
                   </dx:ASPxUploadControl>
                   <br />

                   <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" AutoGenerateColumns="False" KeyFieldName="FPKEY" SettingsPager-Mode="ShowAllRecords" 
                       OnCustomButtonCallback="ASPxGridView1_CustomButtonCallback" OnCustomButtonInitialize="ASPxGridView1_CustomButtonInitialize" SettingsPager-Position="TopAndBottom"
                       Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" Width="100%" Settings-HorizontalScrollBarMode="Auto">
                       <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                            <AdaptiveDetailLayoutProperties colcount="2">
                               <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                            </AdaptiveDetailLayoutProperties>
                       </SettingsAdaptivity>
                       <SettingsResizing ColumnResizeMode="Control" />
                       <%--<ClientSideEvents EndCallback="gridView_EndCallback" />--%>
                       <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                       <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
                       <SettingsBehavior AllowSelectByRowClick="false" FilterRowMode="OnClick" />
                       <Styles>                                                                                      
                            <SelectedRow  />
                            <Row Font-Size="11px" />
                                <DetailCell>
                                    <Paddings PaddingBottom="3px" PaddingTop="3px" />
                                </DetailCell>
                            <AlternatingRow Enabled="True" />
                            <PagerTopPanel Paddings-PaddingBottom="3px">
                                <Paddings PaddingBottom="3px" />
                            </PagerTopPanel>
                        </Styles>
                        <SettingsPopup>
                            <HeaderFilter Height="200px" Width="195px" />
                        </SettingsPopup>
                       <Columns>
                           <dx:GridViewCommandColumn Caption="" VisibleIndex="0" Width="100px">
                               <CustomButtons>                                                                            
                                   <dx:GridViewCommandColumnCustomButton ID="btnEliminar" Text="Eliminar" 
                                       Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                       Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                     
                                       <Styles>
                                           <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                               <HoverStyle Font-Bold="False" ForeColor="Black">
                                               </HoverStyle>
                                           </Style>
                                       </Styles>
                                   </dx:GridViewCommandColumnCustomButton>
                               </CustomButtons>
                               <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                           </dx:GridViewCommandColumn>
                           <dx:GridViewDataTextColumn Caption="FPKEY" FieldName="FPKEY" ReadOnly="True" Visible="False" VisibleIndex="1">
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="PECANAME" FieldName="PECANAME" VisibleIndex="2" Width="260px">
                               <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="STATUSPECA" FieldName="STATUSPECA" VisibleIndex="3"  Width="140px">
                               <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                               <CellStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="OBSERVACIONES" FieldName="OBSERVACIONES" VisibleIndex="4"  Width="250px">
                               <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewCommandColumn Caption="DESCARGAR ARCHIVO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="#FFFFFF" ButtonRenderMode="Image" VisibleIndex="5" Width="130px" Visible="false">
                               <CustomButtons>
                                   <dx:GridViewCommandColumnCustomButton ID="ID_DOWNLOAD" Text=" " Image-ToolTip="Descargar Archivo"
                                       Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                       Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">
                                       <Image Url="../img/iconos/ico_zip.png"></Image>                                                                                
                                       <Styles>                                                                                    
                                           <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                               <HoverStyle Font-Bold="False" ForeColor="Black">
                                               </HoverStyle>
                                           </Style>
                                       </Styles>
                                   </dx:GridViewCommandColumnCustomButton>
                               </CustomButtons>
                           </dx:GridViewCommandColumn>
                           <dx:GridViewCommandColumn Caption="PROCESAR ARCHIVO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="#FFFFFF" ButtonRenderMode="Image" VisibleIndex="6" Width="130px">
                               <CustomButtons>
                                  <dx:GridViewCommandColumnCustomButton ID="ID_PROCESAR" Text=" " Image-ToolTip="Procesar Archivo"
                                       Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                       Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">
                                       <Image Url="../img/iconos/ico_engrane.png"></Image>                                                                                
                                       <Styles>                                                                                    
                                           <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                               <HoverStyle Font-Bold="False" ForeColor="Black">
                                               </HoverStyle>
                                           </Style>
                                       </Styles>
                                   </dx:GridViewCommandColumnCustomButton>
                               </CustomButtons>
                           </dx:GridViewCommandColumn>                           
                       </Columns>
                       <Toolbars>
                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                            <Items>
                                <dx:GridViewToolbarItem Name="Links">
                                <Template>
                                    <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltros_Click" 
                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip"
                                        ToolTip="Limpiar Filtros" CssClasses-Text="txt-sm" >
                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                        <CssClasses Icon="glyphicon glyphicon-erase" /> 
                                    </dx:BootstrapButton>
                                    <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm text-right txt-sm" 
                                        ToolTip="Exportar a Excel" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                    </asp:LinkButton>
                                    <dx:BootstrapButton ID="lkb_Actualizar" runat="server" Text="Actualizar" OnClick="lkb_Actualizar_Click" ToolTip="Actualizar"
                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" CssClasses-Text="txt-sm">
                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                        <CssClasses Icon="glyphicon glyphicon-refresh" /> 
                                    </dx:BootstrapButton>                          
                                </Template>
                               </dx:GridViewToolbarItem>           
                            </Items>
                        </dx:GridViewToolbar>                                               
                       </Toolbars>                       
                   </dx:ASPxGridView>
                   <dx:ASPxGridViewExporter ID="Exporter" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />

            </asp:Panel>
            <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
            </dx:ASPxCallback>
            <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
            </dx:ASPxLoadingPanel>
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