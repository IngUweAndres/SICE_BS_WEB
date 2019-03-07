<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="AdministracionInventarioFisico.aspx.cs" Inherits="InactionWMS_Web_Indar.Presentacion.AdministracionInventarioFisico" %>

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
        //[32] = space
        //[48, 57] = numeros
        //[65, 90] = letras
        //[96, 105] = numeros pad
        //[110] = punto decimal
        //[190] = punto
        function LetrasNumeros(e) {
            var key;
            var isShift;

            if (window.event) {
                key = window.event.keyCode;
                isShift = !!window.event.shiftKey; // typecast to boolean
            }
            else {
                key = e.which;
                isShift = !!e.shiftKey;
            }

            if (isShift && (key >= 48 && key <= 57)) {
                return false;
            }

            if ((key >= 65 && key <= 90) || (key >= 8 && key <= 57) || (key >= 96 && key <= 105) || key == 192 || key == 0)
                return true;

            return false;
        }

        function FitColumns() {
            var grid = $find("<%= RadGrid4.ClientID %>");
            var columns = grid.get_masterTableView().get_columns();
            for (var i = 0; i < columns.length; i++) {
                columns[i].resizeToFit(false, true);
            }
            //document.getElementsByClassName("rgFilterRow")[0].setAttribute("align", "center");
            //PageLoad();
        }

        function pageLoad(sender, args) {
            var grid = $find("<%= RadGrid1.ClientID %>");
            if (grid == null) {
                var grid = $find("<%= RadGrid4.ClientID %>");
                if (grid == null)
                    return;
            }
            //var element = $(grid._element).find('.rgGroupPanel');
            //if (element != null)
            //    element[0].style.display = 'none'

            var masterTable = grid.get_masterTableView();
            if (isFilterAppliedToGrid(masterTable))
                return;
            else
                masterTable.hideFilterItem();
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

        function ShowHideFilterRadGrid4() {
            var masterTable = $find("<%= RadGrid4.ClientID %>");
            if (masterTable == null)
                return;
            masterTable = masterTable.get_masterTableView();
            if (masterTable.get_tableFilterRow().style.display == "none")
                masterTable.showFilterItem();
            else
                masterTable.hideFilterItem()

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

        function RowSelecting(sender, eventArgs) {
            var btnDetalles = document.getElementById('ctl00_ContentSection_RadGrid1_ctl00_ctl02_ctl00_btnDetalles');
            btnDetalles.setAttribute('class', 'btn btn-primary btn-sm');
        }

        function RowDeselecting(sender, eventArgs) {
            var btnDetalles = document.getElementById('ctl00_ContentSection_RadGrid1_ctl00_ctl02_ctl00_btnDetalles');
            var grid = $find("<%=RadGrid1.ClientID %>");
            var checkedRows = grid.get_selectedItems();
            if (checkedRows.length > 0) {
                btnDetalles.setAttribute('class', 'btn btn-primary btn-sm');
            }
            else {
                btnDetalles.setAttribute('class', 'btn btn-primary btn-sm disabled');
            }
        }

        function RadDatePicker1_selectedDate() {
            var picker1 = $find("<%=RadDatePicker1.ClientID%>");
            var picker2 = $find("<%=RadDatePicker2.ClientID%>");

            picker2.set_minDate(picker1.get_selectedDate());
        }

        function RadDatePicker2_selectedDate() {
            var picker1 = $find("<%=RadDatePicker1.ClientID%>");
            var picker2 = $find("<%=RadDatePicker2.ClientID%>");

            picker1.set_maxDate(picker2.get_selectedDate());

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
        <asp:View runat="server">
            <div class="container">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h1 class="panel-title small"><%#tituloPanel%></h1>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="PanelConsultaInventario" runat="server">
                            <div class="form-group row">
                                <div class="col-xs-4 col-md-3 text-center form-group" style="vertical-align: middle;">
                                    <%--<label class="form-control-label">Fecha Inicio:</label>--%>
                                    <telerik:RadDatePicker ID="RadDatePicker1" runat="server" Calendar-ShowRowHeaders="false" DateInput-EmptyMessage="Fecha Inicio..." ToolTip="Fecha Inicio" OnSelectedDateChanged="RadDatePicker1_SelectedDateChanged" Culture="es-MX">
                                        <ClientEvents OnDateSelected="RadDatePicker1_selectedDate" />
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-xs-4 col-md-3 text-center form-group" style="vertical-align: middle;">
                                    <%--<label class="form-control-label">Fecha Fin:</label>--%>
                                    <telerik:RadDatePicker ID="RadDatePicker2" runat="server" Calendar-ShowRowHeaders="false" DateInput-EmptyMessage="Fecha Fin..." ToolTip="Fecha Fin" OnSelectedDateChanged="RadDatePicker2_SelectedDateChanged" Culture="es-MX">
                                        <ClientEvents OnDateSelected="RadDatePicker2_selectedDate" />
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-xs-4 col-md-3 text-center form-group" style="vertical-align: middle;">
                                    <%--<label class="form-control-label">Estatus:</label>--%>
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" DataTextField="IdEstatus" DataValueField="Estatus" EmptyMessage="Estatus..." ToolTip="Estatus" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"></telerik:RadComboBox>
                                </div>
                                <div class="col-xs-12 col-md-3 text-center form-group">
                                    <asp:LinkButton ID="btnConsultar" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnConsultar_Click" ToolTip="Consultar inventarios">
                                        <span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;Buscar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnLimpiar_Click" ToolTip="Limpiar búsqueda">
                                        <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" OnPreRender="RadGrid1_PreRender" OnItemCreated="RadGrid1_ItemCreated" OnPdfExporting="RadGrid1_PdfExporting" AllowFilteringByColumn="true" PageSize="10" AllowPaging="true" AllowSorting="true" ShowFooter="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowGroupPanel="false" EnableLinqExpressions="false">
                                <GroupingSettings CaseSensitive="false" />
                                <GroupPanel Text="Arrastre una columna aquí para agrupar los datos por esa columna."></GroupPanel>
                                <MasterTableView CommandItemDisplay="Top">
                                    <NoRecordsTemplate>No hay registros para mostrar.</NoRecordsTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" Width="180px" />
                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                    <Columns>
                                        <%--<telerik:GridClientSelectColumn UniqueName="ClientSelectColumn1" CommandName="ClientSelectColumn1"
                                            HeaderTooltip="Seleccionar todo" FooterText="Total:">
                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </telerik:GridClientSelectColumn>--%>
                                        <telerik:GridBoundColumn DataField="IdInventario" HeaderText="IdInventario" SortExpression="IdInventario" UniqueName="IdInventario" ReadOnly="true" Display="false" DataType="System.Int32">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Folio" HeaderText="Num. Inventario" SortExpression="Folio" UniqueName="Folio" AutoPostBackOnFilter="true" Aggregate="Count" FooterAggregateFormatString="Total: {0}" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="FechaCreacion" HeaderText="Fecha Creacion" SortExpression="FechaCreacion" UniqueName="FechaCreacion" AutoPostBackOnFilter="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" FilterDateFormat="dd/MM/yyyy" EnableTimeIndependentFiltering="true" FilterControlWidth="100%">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" UniqueName="Estatus" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridTemplateColumn AllowFiltering="False" UniqueName="Detalles" HeaderText="Detalles">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDetalles2" runat="server" CssClass="btn btn-block" CommandName="Detalles" ToolTip="Detalles" CommandArgument='<%# Eval("IdInventario") %>'>
                                                    <span class="glyphicon glyphicon-list-alt"></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table align="left">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Nuevo" ToolTip="Nuevo inventario">
                                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnDetalles" runat="server" CssClass="btn btn-primary btn-sm disabled" CommandName="Detalles" ToolTip="Ver detalles">
                                                        <span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp;Ver Detalles
                                                    </asp:LinkButton>
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
                                    <ClientEvents OnRowSelecting="RowSelecting" OnRowDeselected="RowDeselecting" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="299px" />
                                    <Resizing AllowResizeToFit="true" />
                                </ClientSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Registros:" PagerTextFormat=" {4} {5} registros en {1} páginas" />
                            </telerik:RadGrid>
                            <asp:Timer ID="Timer1" runat="server" Enabled="true" Interval="1" OnTick="Timer1_Tick"></asp:Timer>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server">
            <div class="container" style="max-width: 1100px;">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><%#tituloPanel%></h3>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="PanelNuevoInventario" runat="server">
                            <div class="row form-group">
                                <div class="col-md-9" style="vertical-align: middle;">
                                    <label class="form-control-label">Num. Inventario Preeliminar:&nbsp;</label>
                                    <label class="label-data"><%# numInvPre %> &nbsp;&nbsp;</label>
                                    <%--<label class="form-control-label">Fecha Programación:&nbsp;</label>
                                    <telerik:RadDatePicker ID="rdpFechaProgramacion" runat="server" Calendar-ShowRowHeaders="false" Culture="es-MX">
                                    </telerik:RadDatePicker>--%>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-5">
                                    <div class="list-group">
                                        <h4>Ubicaciones disponibles</h4>
                                        <telerik:RadGrid ID="RadGrid2" runat="server" OnNeedDataSource="RadGrid2_NeedDataSource" OnItemCommand="RadGrid2_ItemCommand" AllowFilteringByColumn="true" PageSize="500" AllowPaging="true" AllowSorting="true" ShowFooter="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowGroupPanel="false">
                                            <GroupingSettings CaseSensitive="false" />
                                            <MasterTableView CommandItemDisplay="None">
                                                <NoRecordsTemplate>No hay registros para mostrar.</NoRecordsTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderTooltip="Seleccionar todo">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn DataField="IdLocalidad" HeaderText="IdLocalidad" SortExpression="IdLocalidad" UniqueName="IdLocalidad" ReadOnly="true" Display="false" DataType="System.Int32">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" UniqueName="Ubicacion" AutoPostBackOnFilter="true" Aggregate="Count" FooterAggregateFormatString="Total: {0}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Almacen" HeaderText="Almacén" SortExpression="Almacen" UniqueName="Almacen" AutoPostBackOnFilter="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Area" HeaderText="Área" SortExpression="Area" UniqueName="Area" AutoPostBackOnFilter="true">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="205px" />
                                            </ClientSettings>
                                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Registros:" PagerTextFormat=" {4} {5} registros en {1} páginas" />
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                                <div class="col-md-2 text-center">
                                    <div class="list-group" style="margin: 50% 0 50%;">
                                        <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnAgregar_Click">
                                        <span class="glyphicon glyphicon-arrow-right"></span>&nbsp;&nbsp;Agregar</asp:LinkButton>
                                        <br />
                                        <br />
                                        <asp:LinkButton ID="btnQuitar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnQuitar_Click">
                                        <span class="glyphicon glyphicon-arrow-left"></span>&nbsp;&nbsp;Quitar&nbsp;&nbsp;&nbsp;</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="list-group">
                                        <h4>Ubicaciones por inventariar</h4>
                                        <telerik:RadGrid ID="RadGrid3" runat="server" OnNeedDataSource="RadGrid3_NeedDataSource" OnItemCommand="RadGrid3_ItemCommand" AllowFilteringByColumn="true" PageSize="500" AllowPaging="true" AllowSorting="true" ShowFooter="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowGroupPanel="false">
                                            <GroupingSettings CaseSensitive="False" />
                                            <MasterTableView CommandItemDisplay="None">
                                                <NoRecordsTemplate>No hay registros para mostrar.</NoRecordsTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderTooltip="Seleccionar todo">
                                                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </telerik:GridClientSelectColumn>
                                                    <telerik:GridBoundColumn DataField="IdLocalidad" HeaderText="IdLocalidad" SortExpression="IdLocalidad" UniqueName="IdLocalidad" ReadOnly="true" Display="false" DataType="System.Int32">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" UniqueName="Ubicacion" AutoPostBackOnFilter="true" Aggregate="Count" FooterAggregateFormatString="Total: {0}">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Almacen" HeaderText="Almacén" SortExpression="Almacen" UniqueName="Almacen" AutoPostBackOnFilter="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Area" HeaderText="Área" SortExpression="Area" UniqueName="Area" AutoPostBackOnFilter="true">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="205px" />
                                            </ClientSettings>
                                            <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Registros:" PagerTextFormat=" {4} {5} registros en {1} páginas" />
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-offset-4 col-md-4" style="text-align: center;">
                                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                                    <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-danger btn-sm" Text="Cancelar" OnClick="btnCancelar_Click">
                                    <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server">
            <div class="container-fluid">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><%#tituloPanel%></h3>
                    </div>
                    <div class="panel-body">
                        <asp:HiddenField ID="HfIdInventario" runat="server" />
                        <asp:HiddenField ID="HfEstatus" runat="server" />
                        <asp:HiddenField ID="HfFolio" runat="server" />
                        <asp:Panel ID="PanelDetallesInventario" runat="server">
                            <telerik:RadGrid ID="RadGrid4" runat="server" OnNeedDataSource="RadGrid4_NeedDataSource" OnItemCommand="RadGrid4_ItemCommand" OnPreRender="RadGrid4_PreRender" OnPdfExporting="RadGrid1_PdfExporting" OnItemCreated="RadGrid1_ItemCreated" AllowFilteringByColumn="true" PageSize="10" AllowPaging="true" AllowSorting="true" ShowFooter="true" AllowMultiRowSelection="true" AutoGenerateColumns="false" ShowGroupPanel="false">
                                <HeaderStyle Width="180px" />
                                <GroupingSettings CaseSensitive="false" />
                                <GroupPanel Text="Arrastre una columna aquí para agrupar los datos por esa columna."></GroupPanel>
                                <MasterTableView CommandItemDisplay="Top">
                                    <NoRecordsTemplate>No hay registros para mostrar.</NoRecordsTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                    <CommandItemSettings ShowExportToWordButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowExportToPdfButton="false" ShowAddNewRecordButton="false" />
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderTooltip="Seleccionar todo">
                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn DataField="IdInventario" HeaderText="IdInventario" SortExpression="IdInventario" UniqueName="IdInventario" AutoPostBackOnFilter="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IdLocalidad" HeaderText="IdLocalidad" SortExpression="IdLocalidad" UniqueName="IdLocalidad" AutoPostBackOnFilter="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IdInventarioConteo" HeaderText="IdInventarioConteo" SortExpression="IdInventarioConteo" UniqueName="IdInventarioConteo" AutoPostBackOnFilter="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" UniqueName="Ubicacion" AutoPostBackOnFilter="true" FooterAggregateFormatString="Total: {0}" Aggregate="Count" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" UniqueName="Estatus" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Codigo" HeaderText="SKU/Código" SortExpression="Codigo" UniqueName="Codigo" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Existencia" HeaderText="Existencia" SortExpression="Existencia" UniqueName="Existencia" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Conteo1" HeaderText="1er Conteo" SortExpression="Conteo1" UniqueName="Conteo1" AutoPostBackOnFilter="true" FooterAggregateFormatString="{0}" Aggregate="Sum" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="Fecha1" HeaderText="1er Fecha" SortExpression="Fecha1" UniqueName="Fecha1" AutoPostBackOnFilter="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" EnableTimeIndependentFiltering="true" FilterControlWidth="100%">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="Conteo2" HeaderText="2do Conteo" SortExpression="Conteo2" UniqueName="Conteo2" AutoPostBackOnFilter="true" FooterAggregateFormatString="{0}" Aggregate="Sum" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="Fecha2" HeaderText="2da Fecha" SortExpression="Fecha2" UniqueName="Fecha2" AutoPostBackOnFilter="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" EnableTimeIndependentFiltering="true" FilterControlWidth="100%">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="Conteo3" HeaderText="3er Conteo" SortExpression="Conteo3" UniqueName="Conteo3" AutoPostBackOnFilter="true" FooterAggregateFormatString="{0}" Aggregate="Sum" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="Fecha3" HeaderText="3ra Fecha" SortExpression="Fecha3" UniqueName="Fecha3" AutoPostBackOnFilter="true" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" EnableTimeIndependentFiltering="true" FilterControlWidth="100%">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="PorAjustar" HeaderText="Por Ajustar" SortExpression="PorAjustar" UniqueName="PorAjustar" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="usuario" HeaderText="Usuario" SortExpression="Usuario" UniqueName="Usuario" AutoPostBackOnFilter="true" FilterControlWidth="100%">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <CommandItemTemplate>
                                        <table align="left">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnRegresar" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Regresar" ToolTip="Regresar a inventarios">
                                                        <span class="glyphicon glyphicon-arrow-left"></span>&nbsp;&nbsp;Regresar
                                                    </asp:LinkButton>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnNuevoConteo" runat="server" CssClass="btn btn-primary btn-sm" CommandName="NuevoConteo" ToolTip="Nuevo conteo">
                                                        <span class="glyphicon glyphicon-edit"></span>&nbsp;&nbsp;Nuevo Conteo
                                                    </asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="¿Desea solicitar un nuevo conteo para las ubicaciones seleccionadas?" TargetControlID="btnNuevoConteo" />
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnAjustar" runat="server" CssClass="btn btn-primary btn-sm" CommandName="Ajustar" ToolTip="Ajustar">
                                                        <span class="glyphicon glyphicon-sort"></span>&nbsp;&nbsp;Ajustar
                                                    </asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="¿Desea solicitar ajuste para las ubicaciones seleccionadas?" TargetControlID="btnAjustar" />
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnCerrarInventario" runat="server" CssClass="btn btn-primary btn-sm" CommandName="CerrarInventario" ToolTip="Cerrar inventario">
                                                        <span class="glyphicon glyphicon-remove-circle"></span>&nbsp;&nbsp;Cerrar Inventario
                                                    </asp:LinkButton>
                                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" ConfirmText="¿Desea cerrar el inventario actual?" TargetControlID="btnCerrarInventario" />
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
                                                    <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-sm" ToolTip="Filtrar" OnClientClick="ShowHideFilterRadGrid4(null, null); return false;">
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
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="360px" />
                                    <Resizing AllowResizeToFit="true" />
                                </ClientSettings>
                                <ExportSettings>
                                    <Pdf PageWidth="297mm" PageHeight="210mm" PageLeftMargin="20" PageTopMargin="20" PageRightMargin="20" PageBottomMargin="20"></Pdf>
                                </ExportSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Registros:" PagerTextFormat=" {4} {5} registros en {1} páginas" />
                            </telerik:RadGrid>
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
            <telerik:AjaxSetting AjaxControlID="PanelConsultaInventario">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelConsultaInventario" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PanelNuevoInventario">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelNuevoInventario" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PanelDetallesInventario">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelDetallesInventario" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
