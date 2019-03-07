<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="AdministracionOrdenSalida.aspx.cs" Inherits="InactionWMS_Web_Indar.Presentacion.AdministracionOrdenSalida" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExcel") >= 0) {
                args.set_enableAjax(false);
            }
            else if (args.get_eventTarget().indexOf("btnPdf") >= 0) {
                args.set_enableAjax(false);
            }
        }

        function pageLoad(sender, args) {
            var grid = $find("<%= RadGrid1.ClientID %>");
            if (grid == null)
                return;
            var masterTable = grid.get_masterTableView();
            if (isFilterAppliedToGrid(masterTable))
                return;
            else
                masterTable.hideFilterItem();
        }

        function isFilterAppliedToGrid(grid) {
            var columns = grid.get_columns();
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].get_filterFunction() != "NoFilter") {
                    return true;
                }
            }
            return false;
        }

        function ShowHideFilterRadGrid1() {
            var masterTable = $find("<%= RadGrid1.ClientID %>");
            if (masterTable == null)
                return;
            masterTable = masterTable.get_masterTableView();
            if (masterTable.get_tableFilterRow().style.display == "none")
                masterTable.showFilterItem();
            else
                masterTable.hideFilterItem();
        }

        function RadGrid1RowSelecting(sender, eventArgs) {
            var btnDetalles = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDetalles").ClientID %>");
            var btnEditar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEditar").ClientID %>");
            var btnEliminar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEliminar").ClientID %>");
            var btnConsolidar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnConsolidar").ClientID %>");
            if (btnDetalles != null)
                btnDetalles.setAttribute('class', 'btn btn-primary btn-sm');
            if (btnEditar != null)
                btnEditar.setAttribute('class', 'btn btn-primary btn-sm');
            if (btnEliminar != null)
                btnEliminar.setAttribute('class', 'btn btn-primary btn-sm');
            if (btnConsolidar != null)
                btnConsolidar.setAttribute('class', 'btn btn-primary btn-sm');
        }

        function RadGrid1RowDeselecting(sender, eventArgs) {
            var btnDetalles = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDetalles").ClientID %>");
            var btnEditar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEditar").ClientID %>");
            var btnEliminar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEliminar").ClientID %>");
            var btnConsolidar = document.getElementById("<%= RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnConsolidar").ClientID %>");
            var grid = $find("<%=RadGrid1.ClientID %>");
            var checkedRows = grid.get_selectedItems();
            if (checkedRows.length > 0) {
                if (btnDetalles != null)
                    btnDetalles.setAttribute('class', 'btn btn-primary btn-sm');
                if (btnEditar != null)
                    btnEditar.setAttribute('class', 'btn btn-primary btn-sm');
                if (btnEliminar != null)
                    btnEliminar.setAttribute('class', 'btn btn-primary btn-sm');
                if (btnConsolidar != null)
                    btnConsolidar.setAttribute('class', 'btn btn-primary btn-sm');
            }
            else {
                if (btnDetalles != null)
                    btnDetalles.setAttribute('class', 'btn btn-primary btn-sm disabled');
                if (btnEditar != null)
                    btnEditar.setAttribute('class', 'btn btn-primary btn-sm disabled');
                if (btnEliminar != null)
                    btnEliminar.setAttribute('class', 'btn btn-primary btn-sm disabled');
                if (btnConsolidar != null)
                    btnConsolidar.setAttribute('class', 'btn btn-primary btn-sm disabled');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
        <asp:View runat="server">
            <div class="container-fluid">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h1 class="panel-title small"><%#tituloPanel%></h1>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="PanelConsulta" runat="server">
                            <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" OnPreRender="RadGrid1_PreRender" OnItemCreated="RadGrid1_ItemCreated" OnPdfExporting="RadGrid1_PdfExporting" AllowFilteringByColumn="true" PageSize="10" AllowPaging="true" AllowSorting="true" ShowFooter="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowGroupPanel="false" EnableLinqExpressions="false">
                                <HeaderStyle Width="180px" />
                                <GroupingSettings CaseSensitive="false" />
                                <GroupPanel Text="Arrastre una columna aquí para agrupar los datos por esa columna."></GroupPanel>
                                <MasterTableView CommandItemDisplay="Top">
                                    <NoRecordsTemplate>No hay registros para mostrar.</NoRecordsTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn1" CommandName="ClientSelectColumn1" HeaderTooltip="Seleccionar todo">
                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn DataField="IdOrdenEmbarque" HeaderText="IdOrdenEmbarque" SortExpression="IdOrdenEmbarque" UniqueName="IdOrdenEmbarque" ReadOnly="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="NumPedido" HeaderText="Num. Orden" SortExpression="NumPedido" UniqueName="NumPedido" AutoPostBackOnFilter="true" Aggregate="Count" FooterAggregateFormatString="Total: {0}" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IdCliente" HeaderText="IdCliente" SortExpression="IdCliente" UniqueName="IdCliente" ReadOnly="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="NombreCliente" HeaderText="Cliente" SortExpression="NombreCliente" UniqueName="NombreCliente" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" UniqueName="Cantidad" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="FechaEntrega" HeaderText="Fecha Entrega" SortExpression="FechaEntrega" UniqueName="FechaEntrega" AutoPostBackOnFilter="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" FilterDateFormat="dd/MM/yyyy" EnableTimeIndependentFiltering="true" FilterControlWidth="100%">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="Consolidado" HeaderText="Consolidado" SortExpression="Consolidado" UniqueName="Consolidado" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Fletera" HeaderText="Fletera" SortExpression="Fletera" UniqueName="Fletera" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="FormaEnvio" HeaderText="Forma Envío" SortExpression="FormaEnvio" UniqueName="FormaEnvio" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Prioridad" HeaderText="Prioridad" SortExpression="Prioridad" UniqueName="Prioridad" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Articulos" HeaderText="Articulos" SortExpression="Articulos" UniqueName="Articulos" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" UniqueName="IdEstatus" Display="false" ReadOnly="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" UniqueName="Estatus" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table align="left">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Nuevo" ToolTip="Nueva orden de salida">
                                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm disabled" CommandName="Editar" ToolTip="Editar orden de salida">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-primary btn-sm disabled" CommandName="Eliminar" ToolTip="Eliminar orden de salida">
                                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnDetalles" runat="server" CssClass="btn btn-primary btn-sm disabled" CommandName="Detalles" ToolTip="Ver detalles">
                                                        <span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;Ver Detalles
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnConsolidar" runat="server" CssClass="btn btn-primary btn-sm disabled" CommandName="Consolidar" ToolTip="Consolidar / separar orden de salida">
                                                        <span class="glyphicon glyphicon-compressed"></span>&nbsp;&nbsp;Consolidar / Separar
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="right">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-primary btn-sm" CommandName="ExportToExcel" ToolTip="Exportar a Excel">
                                                        <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnPdf" runat="server" CssClass="btn btn-primary btn-sm" CommandName="ExportToPdf" ToolTip="Exportar a PDF">
                                                        <span class="glyphicon glyphicon-file"></span>&nbsp;&nbsp;PDF
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-sm" ToolTip="Filtrar" OnClientClick="ShowHideFilterRadGrid1(null, null); return false;">
                                                <span class="glyphicon glyphicon-filter"></span>&nbsp;&nbsp;Filtrar
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnActualizar" runat="server" CssClass="btn btn-primary btn-sm" CommandName="RebindGrid" ToolTip="Actualizar">
                                                        <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </CommandItemTemplate>
                                </MasterTableView>
                                <ClientSettings AllowDragToGroup="true">
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnRowSelecting="RadGrid1RowSelecting" OnRowDeselected="RadGrid1RowDeselecting" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="360px" />
                                    <Resizing AllowResizeToFit="true" />
                                </ClientSettings>
                                <ExportSettings>
                                    <Pdf PageWidth="297mm" PageHeight="210mm"></Pdf>
                                </ExportSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Registros:" PagerTextFormat=" {4} {5} registros en {1} páginas" />
                            </telerik:RadGrid>
                            <asp:Timer ID="Timer1" runat="server" Enabled="true" Interval="1" OnTick="Timer1_Tick"></asp:Timer>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="">
        <div class="RadAjax RadAjax_Default">
            <div class="raDiv">
            </div>
            <div class="raColor raTransp" style="background-color: rgba(245, 245, 245, 0.6); vertical-align: middle;">
                <span style="display: inline-block; height: 100%; vertical-align: middle;"></span>
                <img alt="Loading..." src="../img/refresh.gif" style="border: 0px;" />
            </div>
        </div>
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="MultiView1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="MultiView1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid4" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PanelConsulta">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelConsulta" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PanelNuevo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelNuevo" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PanelDetalles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelDetalles" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
