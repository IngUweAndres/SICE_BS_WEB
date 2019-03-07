<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="DescargaExpediente.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.DescargaExpediente" %>

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





        //Función llamada por método chkAll_Init() al seleccionar o deseleccionar el cuadro del header
        function chkallClick(s, e) {
            var cheked = s.GetChecked();
            var btnDescargar = document.getElementById('ContentSection_btnDescargar')

            if (!btnDescargar.classList.contains('disabled'))
                btnDescargar.classList.add('disabled');

            if (cheked == true)
                btnDescargar.classList.remove('disabled');

            for (var i = 0; i < gridED.GetVisibleRowsOnPage() ; i++) {
                var chkConsultar = eval("chkConsultar" + i);
                chkConsultar.SetChecked(cheked);
            }
        }

        function chkConsultarClick(s, e, index) {
            var cheked = s.GetChecked();
            var btnDescargar = document.getElementById('ContentSection_btnDescargar')

            if (!btnDescargar.classList.contains('disabled'))
                btnDescargar.classList.add('disabled');
           
            for (var i = 0; i < gridED.GetVisibleRowsOnPage() ; i++) {
                var chkConsultar = eval("chkConsultar" + i);
                var valor = chkConsultar.GetChecked();

                if (valor == true) {
                    btnDescargar.classList.remove('disabled');
                    break;
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="View1" runat="server">
    <div class="container-fluid">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
            </div>
            <div class="panel-body" align="center">
                <asp:Panel ID="Panel1" runat="server">
                    <div class="row" style="padding-left: 10px; padding-top: 10px; padding-right: 10px;">
                        <div class="col-sm-4 col-md-2">
                            <div runat="server" id="DivRango">
                                <div class="form-group" style="position: relative; width: 100%; float: left;" title="Rango" data-toggle="tooltip">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon6">
                                            <span class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span>
                                        </span>
                                        <dx:ASPxComboBox ID="RANGO" Caption="" runat="server" Height="30px" NullText="Rango..." DataSecurityMode="Default"
                                            Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact"
                                            OnSelectedIndexChanged="RANGO_SelectedIndexChanged" AutoPostBack="true" ForeColor="#6B5555">
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
                                            <span id="spanDesde" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>
                                        <dx:ASPxDateEdit ID="DESDE" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact"
                                            Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Desde" DisplayFormatString="dd/MM/yyyy">
                                            <CalendarProperties>
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
                                            <span id="spanHasta" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </span>
                                        <dx:ASPxDateEdit ID="HASTA" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact"
                                            Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                            <CalendarProperties>
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
                            <div runat="server" id="DivAduana">
                                <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon2">
                                            <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                        </span>                                         
                                       <dx:ASPxComboBox ID="cmbADUANA" Caption="" runat="server" Height="30px" NullText="Aduana" DataSecurityMode="Default"  
                                           TextField="Aduana" ValueField="Aduana"  Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" 
                                           Theme="MaterialCompact" ForeColor="#6B5555">
                                           <ClearButton DisplayMode="Never" />
                                       </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-2" align="left">
                            <div runat="server" id="DivBuscar">
                                <div class="form-group" style="position: relative; width: 50%; float: left;" title="Buscar Descargas Expediente" data-toggle="tooltip">
                                    <div class="input-group">
                                        <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick"
                                            SettingsBootstrap-RenderOption="Primary" Text="Buscar Descargas Expediente" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                            <CssClasses Icon="glyphicon glyphicon-search" />
                                        </dx:BootstrapButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-2" align="left">
                            <dx:BootstrapButton ID="btnDescargarDS" runat="server" Text="Descarga Data Stage" OnClick="btnDescargarDS_OnClick"
                                SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" 
                                ToolTip="Descarga Data Stage" CssClasses-Text="txt-sm">
                                <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }"
                                     Init="function(s, e) {LoadingPanel1.Hide();}" />
                                <CssClasses Icon="glyphicon glyphicon-download-alt" />
                            </dx:BootstrapButton>
                        </div>
                        <div class="col-sm-1 col-md-10"></div>
                    </div>
                    <div class="col-sm-3"></div>
                    <div class="col-sm-6" style="text-align:left">                        
                        <dx:BootstrapButton ID="btnDescargar" runat="server" Text="Descargar Expediente" OnClick="btnDescargar_OnClick" 
                            SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" 
                            ToolTip="Descargar Expediente" CssClasses-Text="txt-sm">
                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }"
                                 Init="function(s, e) {LoadingPanel1.Hide();}" />
                            <CssClasses Icon="glyphicon glyphicon-download-alt" />
                        </dx:BootstrapButton>                        
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
                        <dx:ASPxGridView ID="GridED" runat="server" EnableTheming="True" Theme="SoftOrange" SettingsPager-Mode="ShowAllRecords"                           
                            EnableCallBacks="false" ClientInstanceName="gridED" AutoGenerateColumns="False" Width="100%"
                            Settings-HorizontalScrollBarMode="Hidden" KeyFieldName="PEDIMENTOARMADO" OnCustomCallback="GridED_CustomCallback" 
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
                                <dx:GridViewDataColumn Caption="TODO" VisibleIndex="0" Width="110px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderTemplate>
                                        <dx:ASPxCheckBox ID="chkAll" ClientInstanceName="chkAll" Enabled="true" runat="server" OnInit="chkAll_Init">
                                        </dx:ASPxCheckBox>
                                    </HeaderTemplate>
                                    <DataItemTemplate>
                                        <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultar_Init">
                                        </dx:ASPxCheckBox>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataTextColumn Caption="PEDIMENTO" FieldName="PEDIMENTO ARMADO" ReadOnly="True" VisibleIndex="1" Width="200px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataTextColumn>                                
                                <dx:GridViewDataDateColumn Caption="FECHA DE PAGO" FieldName="FECHA" ReadOnly="True" VisibleIndex="2" Width="150px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="3" Width="130px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="TIPO DE OPERACION" ReadOnly="True" Visible="false">
                                </dx:GridViewDataTextColumn>
                            </Columns>                                                       
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="Exporter" GridViewID="GridED" runat="server" PaperKind="A5" Landscape="true" />

                        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
                            <ClientSideEvents CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                        </dx:ASPxCallback>
                        <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact">
                        </dx:ASPxLoadingPanel>
                        <dx:ASPxLabel ID="ASPx_Mensaje" runat="server" Text="" Visible="false"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false" />
                    </div>
                    <div class="col-sm-3"></div>
                </asp:Panel>
            </div>
        </div>
    </div>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <div class="container-fluid">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left; width: 50%">
                                <h1 id="h2_titulo" runat="server" class="panel-title small"></h1>
                            </td>
                            <td style="text-align: right; width: 50%">
                                <asp:LinkButton ID="lkb_Regresar" runat="server" OnClick="lkb_Regresar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                     <span class="glyphicon glyphicon-circle-arrow-left"></span>&nbsp;&nbsp;Regresar
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>                        
                </div>
                <div class="panel-body">
                    <div class="col-sm-3"></div>
                    <div class="col-sm-6" style="text-align:left">                                                
                        <dx:BootstrapButton ID="lkb_LimpiarFiltrosDS" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltrosDS_Click"
                            SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip"
                            ToolTip="Limpiar Filtros" CssClasses-Text="txt-sm">
                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }"
                                Init="function(s, e) {LoadingPanel1.Hide();}" />
                            <CssClasses Icon="glyphicon glyphicon-erase" />
                        </dx:BootstrapButton>
                        <asp:LinkButton ID="lkb_ExcelDS" runat="server" OnClick="lkb_ExcelDS_Click" CssClass="btn btn-primary btn-sm text-right txt-sm"
                            ToolTip="Exportar a Excel" data-toggle="tooltip">
                            <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                        </asp:LinkButton>
                        <dx:BootstrapButton ID="lkb_ActualizarDS" runat="server" Text="Actualizar" OnClick="lkb_ActualizarDS_Click" ToolTip="Actualizar"
                            SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" CssClasses-Text="txt-sm">
                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }"
                                Init="function(s, e) {LoadingPanel1.Hide();}" />
                            <CssClasses Icon="glyphicon glyphicon-refresh" />
                        </dx:BootstrapButton>                       
                        <dx:ASPxGridView ID="GridDS" runat="server" EnableTheming="True" Theme="SoftOrange" SettingsPager-Mode="ShowAllRecords"                           
                            EnableCallBacks="false" ClientInstanceName="gridDS" AutoGenerateColumns="False" Width="668px"
                            Settings-HorizontalScrollBarMode="Auto" KeyFieldName="FILESDSKEY"  OnCustomCallback="GridDS_CustomCallback"
                            Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px"  >
                            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800"
                                AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                <AdaptiveDetailLayoutProperties ColCount="2">
                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                </AdaptiveDetailLayoutProperties>
                            </SettingsAdaptivity>
                            <SettingsResizing ColumnResizeMode="Control" />
                            <SettingsBehavior AllowSelectByRowClick="false" />
                            <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false" />
                            <SettingsPopup>
                                <HeaderFilter Height="200px" Width="195px" />
                            </SettingsPopup>
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="FILESDSKEY" ReadOnly="True" VisibleIndex="0" Visible="False">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataColumn Caption="ARCHIVO" VisibleIndex="1" Width="100px">
                                    <EditFormSettings Visible="False" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>                                        
                                        <table>
                                            <tr>                                                
                                                <td>
                                                    <dx:ASPxButton ID="ASPxButtonDoc" runat="server" AutoPostBack="false" OnClick="ASPxButtonDoc_Click"
                                                        EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip='<%# Eval("DSNAME") %>' OnInit="ASPxButtonDoc_Init">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                         </table>
                                    </DataItemTemplate>
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataTextColumn Caption="NOMBRE" FieldName="DSNAME" VisibleIndex="2" Width="240px">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign ="Left" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="DESDE" FieldName="DESDE" ReadOnly="True" VisibleIndex="3" Width="150px">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                    <CellStyle HorizontalAlign="Center" />
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn Caption="HASTA" FieldName="HASTA" ReadOnly="True" VisibleIndex="4" Width="150px">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign="Center" />
                                    <SettingsHeaderFilter Mode="CheckedList" />
                                </dx:GridViewDataDateColumn>                                
                            </Columns>                            
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="Exporter2" GridViewID="GridDS" runat="server" PaperKind="A5" Landscape="true" />
                    </div>
                    <div class="col-sm-3"></div>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>


    <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
    <div class="modal fade" id="AlertError" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-sm" role="document" style="top: 25%; outline: none;">
            <div class="alert alert-danger text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-alert ico"></span>
                <br />
                <br />
                <p id="pModal" runat="server" class="alert-title">
                </p>
                <hr />
                <button type="button" class="btn btn-danger" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
