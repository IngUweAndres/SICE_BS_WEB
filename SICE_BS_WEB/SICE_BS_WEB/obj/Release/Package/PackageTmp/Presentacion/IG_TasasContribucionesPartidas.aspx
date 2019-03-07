<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="IG_TasasContribucionesPartidas.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.IG_TasasContribucionesPartidas" %>

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
                 <div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px;">
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivRango">
                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="Rango" data-toggle="tooltip">
                                <div class="input-group">
                                    <span class="input-group-addon" id="basic-addon6">
                                        <span class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span>
                                    </span>
                                    <dx:ASPxComboBox ID="RANGO" Caption="" runat="server" Height="30px" NullText="Rango..." DataSecurityMode="Default"
                                        Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact"
                                        OnSelectedIndexChanged="RANGO_SelectedIndexChanged" AutoPostBack="true"  ForeColor="#6B5555">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                    Init="function(s, e) {LoadingPanel1.Hide(); }" />
                                        <Items>
                                            <dx:ListEditItem Text="Hoy" Value="Item1" />
                                            <dx:ListEditItem Text="Mes Actual" Value="Item2" />
                                            <dx:ListEditItem Text="Año Actual" Value="Item3" />
                                            <dx:ListEditItem Text="Año pasado" Value="Item4" />
                                            <dx:ListEditItem Text="5 Años" Value="Item5" />
                                        </Items>
                                        <ClearButton DisplayMode="Never" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivDesde">
                            <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" id="FgDesde" title="Desde" data-toggle="tooltip">
                                <div class="input-group">
                                    <span class="input-group-addon" id="basic-addon7">
                                        <span id="spanDesde" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" ></span>
                                    </span>
                                    <dx:ASPxDateEdit ID="DESDE" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                        Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Desde" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties >
                                            <Style Font-Size="12px"></Style>                                                                                    
                                        </CalendarProperties>                                                                                
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivHasta">
                            <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" id="FgHasta" title="Hasta" data-toggle="tooltip">
                                <div class="input-group">
                                    <span class="input-group-addon" id="basic-addon8">
                                        <span id="spanHasta" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" ></span>
                                    </span>
                                     <dx:ASPxDateEdit ID="HASTA" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                        Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties >
                                            <Style Font-Size="12px"></Style>
                                        </CalendarProperties>                                                                                
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivPedimento">
                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="Pedimento" data-toggle="tooltip">
                                <div class="input-group">
                                    <span class="input-group-addon" id="basic-addon1">
                                        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                    </span>
                                    <asp:TextBox ID="PEDIMENTO" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Pedimento"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>     
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivPatente">
                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="Patente" data-toggle="tooltip">
                                <div class="input-group">
                                    <span class="input-group-addon" id="basic-addon5">
                                        <span class="glyphicon glyphicon-option-horizontal" aria-hidden="true"></span>
                                    </span>
                                    <asp:TextBox ID="PATENTES" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Patente"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div> 
                    <div class="col-sm-4 col-md-2">
                        <div runat="server" id="DivBuscar">
                            <div class="form-group" style="position: relative; width: 50%; float: left;" title="Buscar" data-toggle="tooltip">
                                <div class="input-group">
                                    <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick" 
                                        SettingsBootstrap-RenderOption="Primary" Text="Buscar" CssClasses-Text="txt-sm">
                                        <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                    </dx:BootstrapButton>
                                </div>
                            </div>
                        </div>
                    </div> 
                 </div>
                 <br />                                         
                 <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" OnToolbarItemClick="Grid_ToolbarItemClick"
                    OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid" AutoGenerateColumns="False" Width="100%"
                    Settings-HorizontalScrollBarMode="Auto" KeyFieldName="PEDIMENTOARMADO" SettingsPager-Position="TopAndBottom"
                    Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px">
                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                        <AdaptiveDetailLayoutProperties colcount="2">
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                        </AdaptiveDetailLayoutProperties>
                    </SettingsAdaptivity> 
                    <SettingsResizing ColumnResizeMode="Control" />
                    <SettingsBehavior AllowSelectByRowClick="true" />
                    <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                    <Styles>                                                               
                        <SelectedRow CssClass="background_color_btn background_texto_btn" />
                        <Row  />
                        <AlternatingRow Enabled="True" />
                        <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                    </Styles>
                    <SettingsPopup>
                        <HeaderFilter Height="200px" Width="195px"/>
                    </SettingsPopup>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="PEDIMENTO ARMADO" FieldName="PEDIMENTOARMADO" ReadOnly="True" VisibleIndex="1" Width="210px" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="FECHA" FieldName="FECHA" ReadOnly="True" VisibleIndex="2" Width="130px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="TIPO DE OPERACIÓN" FieldName="TIPO DE OPERACION" ReadOnly="True" VisibleIndex="3" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN TIPO DE OPERACIÓN" FieldName="DESCRIPCION TIPO DE OPERACION" ReadOnly="True" VisibleIndex="4" Width="290px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TIPO DE PEDIMENTO" FieldName="TIPO DE PEDIMENTO" ReadOnly="True" VisibleIndex="5" Width="210px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN TIPO PEDIMENTO" FieldName="DESCRIPCION TIPO PEDIMENTO" ReadOnly="True" VisibleIndex="6" Width="280px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PEDIMENTO ORIGINAL" FieldName="PEDIMENTO ORIGINAL" ReadOnly="True" VisibleIndex="7" Width="210px">
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataTextColumn>                        
                        <dx:GridViewDataTextColumn Caption="CLAVE PEDIMENTO" FieldName="CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="8" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN DE LA CLAVE PEDIMENTO" FieldName="DESCRIPCION DE LA CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="9" Width="530px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="PARTIDA" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="10" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FRACCIÓN" FieldName="FRACCIÓN" ReadOnly="True" VisibleIndex="11" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN FRACCIÓN" FieldName="DESCRIPCIÓN FRACCIÓN" ReadOnly="True" VisibleIndex="12" Width="1200px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CLAVE GRAVAMEN" FieldName="CLAVE GRAVAMEN" ReadOnly="True" VisibleIndex="13" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN GRAVAMEN" FieldName="DESCRIPCIÓN GRAVAMEN" ReadOnly="True" VisibleIndex="14" Width="530px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TASA CLAVE" FieldName="TASA CLAVE" ReadOnly="True" VisibleIndex="15" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN TASA" FieldName="DESCRIPCIÓN TASA" ReadOnly="True" VisibleIndex="16" Width="530px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TASA APLICABLE" FieldName="TASA APLICABLE" ReadOnly="True" VisibleIndex="17" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>                       
                                               
                        <dx:GridViewDataTextColumn Caption="CLAVE PAÍS DESTINO" FieldName="CLAVE PAIS DESTINO" ReadOnly="True" VisibleIndex="19" Width="210px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PAÍS DESTINO" FieldName="PAIS DESTINO" ReadOnly="True" VisibleIndex="20" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CLAVE ADUANA" FieldName="CLAVE ADUANA" ReadOnly="True" VisibleIndex="21" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ADUANA" FieldName="ADUANA" ReadOnly="True" VisibleIndex="22" Width="260px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PESO BRUTO" FieldName="PESO BRUTO" ReadOnly="True" VisibleIndex="23" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="TIPO DE CAMBIO" FieldName="TIPO DE CAMBIO" ReadOnly="True" VisibleIndex="24" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>                        
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE SALIDA" FieldName="MEDIO DE TRANSPORTE SALIDA" ReadOnly="True" VisibleIndex="25" Width="260px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE SALIDA DESCRIPCIÓN" FieldName="MEDIO DE TRANSPORTE SALIDA DESCRIPCION" ReadOnly="True" VisibleIndex="26" Width="360px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE ARRIBO" FieldName="MEDIO DE TRANSPORTE ARRIBO" ReadOnly="True" VisibleIndex="27" Width="260px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE ARRIBO DESCRIPCIÓN" FieldName="MEDIO DE TRANSPORTE ARRIBO DESCRIPCION" ReadOnly="True" VisibleIndex="28" Width="360px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE ENTRADA" FieldName="MEDIO DE TRANSPORTE ENTRADA" ReadOnly="True" VisibleIndex="29" Width="260px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="MEDIO DE TRANSPORTE ENTRADA DESCRIPCIÓN" FieldName="MEDIO DE TRANSPORTE ENTRADA DESCRIPCION" ReadOnly="True" VisibleIndex="30" Width="360px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CURP APODERADO MANDATARIO" FieldName="CURP APODERADO MANDATARIO" ReadOnly="True" VisibleIndex="31" Width="260px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>                                              
                        <dx:GridViewDataTextColumn Caption="RFC AGENTE ADUANAL" FieldName="RFC AGENTE ADUANAL" ReadOnly="True" VisibleIndex="32" Width="230px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="VALOR DÓLARES" FieldName="VALOR DOLARES" ReadOnly="True" VisibleIndex="33" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="VALOR ADUANAL" FieldName="VALOR ADUANAL" ReadOnly="True" VisibleIndex="34" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="VALOR COMERCIAL" FieldName="VALOR COMERCIAL" ReadOnly="True" VisibleIndex="35" Width="190px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <SettingsHeaderFilter Mode="CheckedList" />
                        </dx:GridViewDataTextColumn>                       
                    </Columns>                    
                    <Toolbars>
                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                            <Items>
                                <dx:GridViewToolbarItem Name="Links">
                                <Template>
                                    <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                        <CssClasses Icon="glyphicon glyphicon-search" /> 
                                    </dx:BootstrapButton>--%>
                                    <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltros_Click" 
                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip"
                                        ToolTip="Limpiar Filtros" CssClasses-Text="txt-sm">
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
                                    <%--
                                    <asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltros_Click" CssClass="btn btn-primary btn-sm" ToolTip="Limpiar Filtros" >
                                        <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar Filtros                                        
                                    </asp:LinkButton>                                    
                                    <asp:LinkButton ID="lkb_Actualizar" runat="server" OnClick="lkb_Actualizar_Click" CssClass="btn btn-primary btn-sm text-right" ToolTip="Actualizar">
                                         <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                    </asp:LinkButton>
                                    --%>                            
                                </Template>
                               </dx:GridViewToolbarItem>           
                            </Items>
                        </dx:GridViewToolbar>                                               
                    </Toolbars>
                    <Templates>
                        <PagerBar>
                           <table runat="server" class="OptionsTable" style="width: 100%">
                               <tr>
                                   <td style="padding-left: 8px;">
                                       <dx:BootstrapButton ID="btnInicial" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                           <ClientSideEvents Click="function() { grid.GotoPage(0) }" />
                                           <CssClasses Icon="glyphicon glyphicon-backward" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnAtras" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                           <ClientSideEvents Click="function() { grid.PrevPage() }" />
                                           <CssClasses Icon="glyphicon glyphicon-triangle-left" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:ASPxTextBox runat="server" ID="GotoBox" Width="30" Font-Size="11px">
                                           <ClientSideEvents Init="CustomPager.gotoBox_Init" ValueChanged="CustomPager.gotoBox_ValueChanged"
                                               KeyPress="CustomPager.gotoBox_KeyPress" />
                                       </dx:ASPxTextBox>
                                   </td>
                                   <td style="padding-left: 5px;font-size:11px">
                                       <span style="white-space: nowrap">
                                           de <%# Container.Grid.PageCount %>
                                       </span>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnSiuiente" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                           <ClientSideEvents Click="function() { grid.NextPage() }" />
                                           <CssClasses Icon="glyphicon glyphicon-triangle-right" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnFinal" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                           <ClientSideEvents Click="function() { grid.GotoPage(grid.GetPageCount() - 1);}" />
                                           <CssClasses Icon="glyphicon glyphicon-forward" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="width: 100%; padding-left:5px;font-size:11px">
                                       <span style="white-space: nowrap">
                                           (<%# Container.Grid.VisibleRowCount %>)
                                       </span>
                                   </td>
                                   <td style="white-space: nowrap">
                                       <span style="white-space: nowrap;font-size:11px">
                                           Filas:
                                       </span>
                                   </td>
                                   <td style="padding-right: 5px;">
                                       <dx:ASPxComboBox runat="server" ID="Combo" Width="70px" DropDownWidth="70px" ValueType="System.Int32"
                                           OnLoad="PagerCombo_Load" Font-Size="11px">
                                           <Items>
                                               <dx:ListEditItem Value="20" />
                                               <dx:ListEditItem Value="30" />
                                               <dx:ListEditItem Value="50" />
                                           </Items>
                                           <ClientSideEvents SelectedIndexChanged="CustomPager.combo_SelectedIndexChanged" />
                                       </dx:ASPxComboBox>
                                   </td>
                               </tr>
                           </table>
                       </PagerBar>
                    </Templates>                      
                 </dx:ASPxGridView>
                 <dx:ASPxGridViewExporter ID="Exporter" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />                                              
                 </asp:Panel>                       
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

    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
