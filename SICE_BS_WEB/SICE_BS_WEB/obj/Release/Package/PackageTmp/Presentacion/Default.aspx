<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Default" %>

<%@ Register assembly="DevExpress.XtraCharts.v17.1.Web, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v17.1, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<%@ Register assembly="DevExpress.XtraCharts.v17.1.Web, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web.Designer" tagprefix="dxchartdesigner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
       
        .txt-size12{
            font-size:12px;
        }

        /*Diseño y función de los botones superiores de las gráficas*/
        .stat-row {
            position: relative;
            width: 100%;
            z-index: 100;            
            border-top: 1px solid #E3E3E3;
            border-bottom: 1px solid #E3E3E3;
            background: #ffffff;
            background-image: -moz-linear-gradient(top, #ffffff, #f7f7f7);
            background-image: -ms-linear-gradient(top, #ffffff, #f7f7f7);
            background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#ffffff), to(#f7f7f7));
            background-image: -webkit-linear-gradient(top, #ffffff, #f7f7f7);
            background-image: -o-linear-gradient(top, #ffffff, #f7f7f7);
            background-image: linear-gradient(top, #ffffff, #f7f7f7);

        }

        .stat-set {
            text-align: center;
            border-left: 1px solid #E3E3E3;
            padding-top: 5px;
            padding-bottom: 5px;
            cursor: pointer;
            width: 16.666666667% !important;
            margin-left: -17px;
            margin-right: 25px;
            float: left;
            min-height: 70px;
            height: 70px;
        }
        .stat-set.selected {
            background: #efefef;
            background-color: rgb(239, 239, 239);
            background-image: none;
            background-repeat: repeat;
            background-attachment: scroll;
            background-clip: border-box;
            background-origin: padding-box;
            background-position-x: 0%;
            background-position-y: 0%;
            background-size: auto auto;
        }
        .stat-set .title {
            font-size: 11px;
            color: #777;
            display: block;
            line-height: 17px;
        }
        .stat-set .units {
            font-size: 11px;
            color: #BBB;
            display: block;
            white-space: nowrap;
            width: 100%;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        .chart-button-value {
            display: block;
            font-size: 11px;
            font-weight: 500;
            line-height: 15px;
        }

        .clear {
            clear: both;
        }

        .renglon {
          margin-right: -5px;
          margin-left: -10px;
        }

        .titulo-grafica-fecha{
            font-size:12px;
            font-family:Arial;
        }

        .titulo-grafica{
            font-size:24px;
            font-family:Arial;
        }
    </style>    
    <script type="text/javascript">
        //Inicializa todos los tooltip
        $(function () {
            $('[data-toggle="popover"]').popover();
            $('[data-toggle="tooltip"]').tooltip({
                delay: 500,
                trigger: 'hover'
            });
        });

        function BootstrapChart1_onPointClick(s, e) {
            e.target.select();
        }

        //Gráfica Tipo Operaciones
        function BootstrapChart1_onPointClick(s, e) {
            e.target.select();
        }

        //Gráfica Tipo Operaciones
        function BootstrapChart1_CustomizeToolTip(args) {
            return {
                html: "<div>" +
                      "<h6>" + "Operaciones: " + args.value + "</h6>" +
                      "<h6>" + "Patente: " + args.argument + "</h6>" +
                      "</div>"
            };
        }

        //Gráfica Valor Operaciones
        function BootstrapChart2_CustomizeToolTip(args) {
            return {
                html: "<div>" +
                      "<h6>" + "Valor: " + args.value + "</h6>" +
                      "<h6>" + "Patente: " + args.argument + "</h6>" +
                      "</div>"
            };
        }

        //Botones (Coves, Coves Descargados, Edocs, Edocs Descargados) para el code behind
        function btnGrafica(s, e) {
            var id = $(s).attr("id");
            __doPostBack(id, id);
        }

        //Tooltip Gráfica Área
        function customizeTooltipArea(args) {
            return {
                html: "<div>" +
                      "<h6>" + "Total: " + args.value + "</h6>" +
                      "<h6>" + "Fecha: " + args.argument + "</h6>" +
                      "</div>"
            };
        }

        //Tooltip Gráfica Dona
        function customizeTooltipDona(args) {
            return {
                html: "<div>" +
                     "<h6>" + "Total: " + args.value + "</h6>" +
                     "<h6>" + "Estatus: " + args.argument + "</h6>" +
                     "</div>"
            };
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">

<%--<div class="container" style="text-align: center;">
    <div >
        <h2 class="display-3"">B I E N V E N I D @</h2>
        <h2 class="display-3"">A</h2>
        <h2 class="display-3"">SICE VUCEM</h2>
        <p class="lead">Si desea conocer más acerca del sistema de clic en el siguiente botón</p>
        <hr class="my-4">
        <p class="lead">
            <a class="btn btn-primary btn-lg" href="AcercaDe.aspx" role="button">Acerca de</a>
        </p>
    </div>
</div>--%>

<div class="container-fluid" >
    <div class="panel-body bordes_curvos" style="background-color:#f8f8f8" >         
        <div class="col-sm-12" style="padding-bottom:5px">
            <dx:ASPxLabel ID="lblT_Informes" runat="server" Text="Informes" Font-Size="20px" />
        </div>
        <dx:ASPxPageControl runat="server" ID="ASPxPageControlCG" Height="100%" Width="100%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
             TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
             <TabStyle Paddings-PaddingLeft="50px" Paddings-PaddingRight="50px"  >                                                                                      
                <Paddings PaddingLeft="50px" PaddingRight="50px"></Paddings>
             </TabStyle>
             <ContentStyle>
                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
             </ContentStyle>
             <TabPages>
                 <dx:TabPage Text="Información General" >
                     <ContentCollection>
                     <dx:ContentControl ID="ContentControl1" runat="server">
                        <div class="renglon form-group input-group" style="padding-left:5px; padding-top:0px; padding-right:5px; width:100%">
                            <asp:Panel ID="PanelInfo" runat="server" DefaultButton="btnActualizar">
                            <br />
                            <div class="col-sm-4 col-md-2">
                                <div runat="server" id="DivPInfo">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" title="Periodo del Informe" data-toggle="tooltip">
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon6">
                                                <span class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span>
                                            </span>
                                            <dx:ASPxComboBox ID="PERIODO" Caption="" runat="server" Height="30px" NullText="Periodo del Informe" DataSecurityMode="Default"
                                                Width="100%" Font-Size="11px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact"
                                                 AutoPostBack="true"  ForeColor="#6B5555" OnSelectedIndexChanged="PERIODO_SelectedIndexChanged">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                            Init="function(s, e) {LoadingPanel1.Hide(); }" />
                                                <Items>
                                                    <dx:ListEditItem Text="Últimos 30 días" Value="1" Selected="true" />
                                                    <dx:ListEditItem Text="Últimos 7 días" Value="2" />                                                    
                                                    <dx:ListEditItem Text="Personalizado" Value="3" />
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
                                                <span id="spanDesde" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" style="width:100%"></span>
                                            </span>
                                            <dx:ASPxDateEdit ID="DESDE" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                                Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Desde" DisplayFormatString="dd/MM/yyyy">
                                                <CalendarProperties >
                                                    <Style Font-Size="11px"></Style>                                                                                    
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
                                                <span id="spanHasta" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" style="width:100%" ></span>
                                            </span>
                                             <dx:ASPxDateEdit ID="HASTA" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                                Font-Size="12px" CssClass="bordes_curvos_derecha" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                                <CalendarProperties >
                                                    <Style Font-Size="11px"></Style>
                                                </CalendarProperties>                                                                                
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>
                            </div>                     
                            <div class="col-sm-2 col-md-3"> 
                                <div runat="server" id="DivActualizar">
                                    <div class="form-group" style="position: relative; width: 30%; float: left;" title="Actualizar" data-toggle="tooltip">
                                        <div class="input-group txt-size12">
                                            <dx:BootstrapButton ID="btnActualizar" runat="server" AutoPostBack="false" OnClick="btnActualizar_Click"
                                                SettingsBootstrap-RenderOption="Primary" Text="Actualizar" CssClasses-Text="txt-sm">
                                                <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                            </dx:BootstrapButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-10 col-md-3"> 
                                <div runat="server" id="DivVacio1">
                                    <div class="form-group" style="position: relative; width: 100%; float: left"><div class="input-group"></div></div>
                                </div>
                            </div>
                            </asp:Panel>
                        </div>                 
                        <div class="renglon" style="width:100%;">
                            <%--Gráfica Estadística de Coves y Edocuments--%>
                            <%--<div class="col-sm-12" style="margin-bottom:20px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:-1px;padding-bottom:15px; border-color:#B4EEFC; border-width:1px;border-style:solid">
                                    <div class="col-xs-12 stat-row" >
                                        <div id="divCoves" runat="server" class="stat-set" onclick="btnGrafica(this)" style="position: relative; width: 100%; float: left;" data-toggle="tooltip" title="Total Coves"> 
                                            <span id="sCoves" runat="server" class="chart-button-value" style="color: inherit;">0</span>
                                            <span class="title">Coves</span>
                                            <span class="units">(total)</span>
                                        </div>
                                        <div id="divDesCoves" runat="server" class="stat-set" onclick="btnGrafica(this)" data-toggle="tooltip" title="Total Descargos Coves">
                                            <span id="sCovesD" runat="server" class="chart-button-value" style="color: inherit;">0</span>
                                            <span class="title">Coves Desc.</span>
                                            <span class="units">(total)</span>
                                        </div>
                                        <div id="divEdocs" runat="server" class="stat-set" onclick="btnGrafica(this)" data-toggle="tooltip" title="Total Edocuments">
                                            <span id="sEdocs" runat="server" class="chart-button-value" style="color: inherit;">0</span>
                                            <span class="title">Edocs</span>
                                            <span class="units">(total)</span>
                                        </div>
                                        <div id="divDesEdocs" runat="server" class="stat-set" onclick="btnGrafica(this)" data-toggle="tooltip" title="Total Descargos Edocuments">
                                            <span id="sEdocsD" runat="server" class="chart-button-value" style="color: inherit;">0</span>
                                            <span class="title">Edocs Desc.</span>
                                            <span class="units">(total)</span>
                                        </div>
                                        <div class="stat-set">
                                        </div>
                                    </div>       
                                    <br /><br /><br /><br /><br /><br />--%>
                            <%--<dx:WebChartControl ID="WebChartControl1" runat="server" CrosshairEnabled="True" Height="310px" Width="1148px"
                                        AppearanceNameSerializable="Chameleon" AutoLayout="True">
                                        <DiagramSerializable>
                                            <dx:XYDiagram>
                                                <AxisX VisibleInPanesSerializable="-1">
                                                    <VisualRange Auto="False" AutoSideMargins="False" MaxValueSerializable="9" MinValueSerializable="0" SideMarginsValue="0" />
                                                    <WholeRange AutoSideMargins="False" SideMarginsValue="0" />
                                                </AxisX>
                                                <AxisY VisibleInPanesSerializable="-1">
                                                </AxisY>
                                            </dx:XYDiagram>
                                        </DiagramSerializable>
                                        <Legend Name="Default Legend"></Legend>
                                        <SeriesSerializable>
                                            <dx:Series Name="COVE" ArgumentDataMember="Fecha" ToolTipPointPattern="{A:dd/MM/yyyy}" ToolTipSeriesPattern="{S:dd/MM/yyyy}" 
                                                ValueDataMembersSerializable="TOTAL COVES">
                                                <ViewSerializable>
                                                    <dx:AreaSeriesView>
                                                    </dx:AreaSeriesView>
                                                </ViewSerializable>
                                            </dx:Series>
                                        </SeriesSerializable>
                                        <SeriesTemplate>
                                            <ViewSerializable>
                                                <dx:AreaSeriesView>
                                                </dx:AreaSeriesView>
                                            </ViewSerializable>
                                        </SeriesTemplate>
                                    </dx:WebChartControl>--%>
                            <%--<div class="col-sm-12">
                                        <dx:ASPxLabel ID="lbl_Tit_Estaditica" runat="server" Text="Estadística de Coves y Edocuments" Font-Size="15px" Font-Names="Arial,Helvetica" />
                                        <dx:BootstrapChart ID="ChartArea" runat="server" ZoomingMode="All" Visible="true" ScrollingMode="All" CustomPalette="#41b6e3">
                                            <SettingsCommonSeries Point-Symbol="Circle"></SettingsCommonSeries>
                                            <SettingsScrollBar Position="Bottom" Visible="true" />
                                            <SettingsToolTip Enabled="true" OnClientCustomizeTooltip="customizeTooltipArea" />
                                            <SettingsLegend VerticalAlignment="Top" HorizontalAlignment="Center" />                                       
                                        </dx:BootstrapChart> 
                                    </div>
                                </div>
                            </div>--%>
                            <%--Gráfica Operaciones y Patentes--%>
                            <div class="col-xs-12" style="margin-bottom:20px;height:450px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-left:0px;padding-right:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl1_Tit_Tipo_Operaciones" runat="server" Text="Total de Operaciones" CssClass="titulo-grafica" />
                                    </div>
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl1_Del" runat="server" Text="Del" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl1_FechaIni" runat="server" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl1_al" runat="server" Text="al" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl1_FechaFin" runat="server" CssClass="titulo-grafica-fecha" />
                                    </div>
                                    <dx:BootstrapChart ID="BootstrapChart1" runat="server" PointSelectionMode="Single" ZoomingMode="None" ScrollingMode="None"
                                        SettingsScrollBar-Position="Bottom" SettingsScrollBar-Visible="false" CustomPalette="#558ED5, #93CDDD" > 
                                        
                                        <SettingsToolTip ArgumentFormat-Type="Currency" Enabled="true" OnClientCustomizeTooltip="BootstrapChart1_CustomizeToolTip" />
                                        <ValueAxisCollection>
                                            <dx:BootstrapChartValueAxis TitleText="Operaciones" Visible="true">
                                                <%--<Label>
                                                    <Format Type="Currency" Precision="1" />
                                                </Label>--%>
                                            </dx:BootstrapChartValueAxis>                                       
                                        </ValueAxisCollection>
                                        <ArgumentAxis TitleText="Patentes" Visible="true" TickInterval="2000"></ArgumentAxis>                                       
                                        <SeriesCollection>
                                            <dx:BootstrapChartBarSeries ArgumentField="IMPORTACION" ValueField="OPERACIONES" Name="Importación">
                                                <Label Visible="true" />
                                            </dx:BootstrapChartBarSeries> 
                                            <dx:BootstrapChartBarSeries ArgumentField="EXPORTACION" ValueField="OPERACIONES" Name="Exportación">
                                                <Label Visible="true" />
                                            </dx:BootstrapChartBarSeries>
                                        </SeriesCollection>                                        
                                        <SettingsLegend VerticalAlignment="Bottom" HorizontalAlignment="Center" />
                                        <SettingsToolTip Enabled="true" ArgumentFormat-Type="Currency" ArgumentFormat-Precision="1" />
                                    </dx:BootstrapChart>                                    
                                </div>
                            </div>
                            <%--Gráfica Valor de Operaciones--%>
                            <div class="col-xs-12" style="margin-bottom:20px;height:450px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-right:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl2_Tit_Valor_Operaciones" runat="server" Text="Valor de Operaciones" CssClass="titulo-grafica" />
                                    </div>
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl2_Del" runat="server" Text="Del" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl2_FechaIni" runat="server" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl2_al" runat="server" Text="al" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl2_FechaFin" runat="server" CssClass="titulo-grafica-fecha" />
                                    </div>
                                    <dx:BootstrapChart ID="BootstrapChart2" runat="server" PointSelectionMode="Single" ZoomingMode="None" ScrollingMode="None"
                                        SettingsScrollBar-Position="Bottom" SettingsScrollBar-Visible="false" CustomPalette="#daba25, #b90d0d" >  <%--"#CEFF00, #00FF63"--%>
                                        <SettingsToolTip ArgumentFormat-Type="Currency" Enabled="true" OnClientCustomizeTooltip="BootstrapChart2_CustomizeToolTip" />
                                        <ValueAxisCollection>
                                            <dx:BootstrapChartValueAxis TitleText="Valores" ValueType="System.Decimal" Visible="true"/>                                       
                                        </ValueAxisCollection>
                                        <ArgumentAxis TitleText="Patentes" Visible="true"></ArgumentAxis>                                       
                                        <SeriesCollection>
                                            <dx:BootstrapChartBarSeries ArgumentField="IMPORTACION" ValueField="VALOR" Name="Importación">
                                                <Label Visible="true" />
                                            </dx:BootstrapChartBarSeries> 
                                            <dx:BootstrapChartBarSeries ArgumentField="EXPORTACION" ValueField="VALOR" Name="Exportación">
                                                <Label Visible="true" />
                                            </dx:BootstrapChartBarSeries>
                                        </SeriesCollection>                                        
                                        <SettingsLegend VerticalAlignment="Bottom" HorizontalAlignment="Center" />
                                        <SettingsToolTip Enabled="true"/>
                                    </dx:BootstrapChart>
                                </div>
                            </div>
                            <%--Gráfica Sin información por el momento...--%>
                            <%--<div class="col-sm-6" style="margin-bottom:20px;height:450px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-left:0px;padding-right:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">
                                    <br /><br /><br /><br /><br /><br /><br />
                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Sin información por el momento..." Font-Size="12px" />
                                </div>
                            </div>--%>
                            <%--Gráfica Sin información por el momento...--%>
                            <%--<div class="col-sm-6" style="margin-bottom:20px;height:450px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">
                                    <br /><br /><br /><br /><br /><br /><br />
                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Sin información por el momento..." Font-Size="12px" />
                                </div>
                            </div>--%>

                            <%--Gráfica Estatus de Coves--%>
                            <div class="col-sm-4 col-md-4" style="margin-bottom:20px;height:480px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-left:0px;padding-right:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">                                                                         
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl5_Tit_EstatusCoves" runat="server" Text="Estatus de Coves" CssClass="titulo-grafica" />
                                    </div>
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl5_Del" runat="server" Text="Del" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl5_FechaIni" runat="server" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl5_al" runat="server" Text="al" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl5_FechaFin" runat="server" CssClass="titulo-grafica-fecha" />
                                    </div>
                                    <div class="renglon" style="width:100%;">&nbsp;</div>
                                    <div class="renglon" style="width:100%;">
                                        <asp:LinkButton ID="lkb_CovePedimento" runat="server"  PostBackUrl="~/Presentacion/IV_CovePedimento.aspx" ForeColor="#000">
                                            <img src="../img/iconos/015-financial.png" width="30px" height="30px"  />
                                            Cove Pedimento
                                        </asp:LinkButton>
                                    </div>
                                    <dx:WebChartControl ID="WebChartControlDona" runat="server" CrosshairEnabled="True" Height="370px" Width="270px" PaletteName="Palette 1" 
                                        AppearanceNameSerializable="Chameleon" AutoLayout="True">
                                        <BorderOptions Visibility="False" />
                                        <DiagramSerializable>
                                            <dx:SimpleDiagram Dimension="6">
                                                <Margins Left="5" Right="5" />
                                            </dx:SimpleDiagram>
                                        </DiagramSerializable>
                                        <Legend Name="Default Legend" AlignmentHorizontal="Center" AlignmentVertical="BottomOutside" Direction="LeftToRight" EnableAntialiasing="True" 
                                            Font="Arial, 9pt" Visibility="True" MarkerOffset="6" MaxVerticalPercentage="10" BackColor="244, 249, 253">
                                            <Margins Bottom="0" Left="0" Right="0" Top="0" />
                                            <Border Visibility="False" />
                                            <Title Text="STATUS" Font="Arial, 11pt" TextColor="79, 129, 189"></Title></Legend>
                                        <SeriesSerializable>
                                            <dx:Series ArgumentDataMember="STATUS" Name="STATUS" ValueDataMembersSerializable="TOTAL" LegendName="STATUS" 
                                                SeriesPointsSorting="Descending" SeriesPointsSortingKey="Value_1" ToolTipEnabled="True" ToolTipPointPattern="{A} : {V:#,#}" 
                                                ToolTipHintDataMember="STATUS" >
                                                <ViewSerializable>
                                                    <dx:DoughnutSeriesView HoleRadiusPercent="50" ExplodedDistancePercentage="5" ExplodeMode="UsePoints">
                                                        <FillStyle FillMode="Gradient">
                                                        </FillStyle>
                                                    </dx:DoughnutSeriesView>
                                                </ViewSerializable>
                                                <LabelSerializable>
                                                    <dx:DoughnutSeriesLabel Font="Arial, 8pt" MaxLineCount="2" LineVisibility="True" TextAlignment="Near">
                                                        <Shadow Visible="True" />
                                                    </dx:DoughnutSeriesLabel>
                                                </LabelSerializable>
                                                <TopNOptions Enabled="True" />
                                            </dx:Series>
                                        </SeriesSerializable>
                                        <PaletteWrappers>
                                            <dx:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <Palette>
                                                    <dx:PaletteEntry Color="0, 163, 227" Color2="0, 163, 227" />
                                                    <dx:PaletteEntry Color="183, 205, 36" Color2="183, 205, 36" />
                                                    <dx:PaletteEntry Color="100, 84, 158" Color2="100, 84, 158" />
                                                    <dx:PaletteEntry Color="95, 242, 250" Color2="95, 242, 250" />
                                                    <dx:PaletteEntry Color="252, 133, 154" Color2="252, 133, 154" />
                                                </Palette>
                                            </dx:PaletteWrapper>
                                        </PaletteWrappers>
                                    </dx:WebChartControl>
                                </div>
                            </div>
                            <%--Gráfica Estatus de Edocuments--%>
                            <div class="col-sm-4 col-md-4" style="margin-bottom:20px;height:480px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px;padding-left:0px;padding-right:15px;border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">                                    
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl6_Tit_EstatusED" runat="server" Text="Estatus de Edocuments" CssClass="titulo-grafica" />
                                    </div>
                                    <div class="renglon" style="width:100%;">
                                        <dx:ASPxLabel ID="lbl6_Del" runat="server" Text="Del" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl6_FechaIni" runat="server" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl6_al" runat="server" Text="al" CssClass="titulo-grafica-fecha"/>&nbsp;
                                        <dx:ASPxLabel ID="lbl6_FechaFin" runat="server" CssClass="titulo-grafica-fecha" />
                                    </div>
                                    <div class="renglon" style="width:100%;">&nbsp;</div>
                                    <div class="renglon" style="width:100%;">
                                        <asp:LinkButton ID="lkb_Edocument" runat="server"  PostBackUrl="~/Presentacion/IV_Edocuments.aspx" ForeColor="#000">
                                            <img src="../img/iconos/014-globalization.png" width="32px" height="32px"  />
                                            Edocuments
                                        </asp:LinkButton>
                                    </div>
                                    <dx:WebChartControl ID="WebChartControlDona2" runat="server" CrosshairEnabled="True" Height="370px" Width="270px" PaletteName="Palette 1" 
                                        AppearanceNameSerializable="Chameleon" AutoLayout="True">
                                        <BorderOptions Visibility="False" />
                                        <DiagramSerializable>
                                            <dx:SimpleDiagram Dimension="6">
                                                <Margins Left="5" Right="5" />
                                            </dx:SimpleDiagram>
                                        </DiagramSerializable>
                                        <Legend Name="Default Legend" AlignmentHorizontal="Center" AlignmentVertical="BottomOutside" Direction="LeftToRight" EnableAntialiasing="True" 
                                            Font="Arial, 9pt" Visibility="True" MarkerOffset="6" MaxVerticalPercentage="10" BackColor="244, 249, 253">
                                            <Margins Bottom="0" Left="0" Right="0" Top="0" />
                                            <Border Visibility="False" />
                                            <Title Text="STATUS" Font="Arial, 11pt" TextColor="79, 129, 189"></Title></Legend>
                                        <SeriesSerializable>
                                            <dx:Series ArgumentDataMember="STATUS" Name="STATUS" ValueDataMembersSerializable="TOTAL" LegendName="STATUS" 
                                                SeriesPointsSorting="Descending" SeriesPointsSortingKey="Value_1" ToolTipEnabled="True" ToolTipPointPattern="{A} : {V:#,#}" 
                                                ToolTipHintDataMember="STATUS" >
                                                <ViewSerializable>
                                                    <dx:DoughnutSeriesView HoleRadiusPercent="50" ExplodedDistancePercentage="5" ExplodeMode="UsePoints">
                                                        <FillStyle FillMode="Gradient">
                                                        </FillStyle>
                                                    </dx:DoughnutSeriesView>
                                                </ViewSerializable>
                                                <LabelSerializable>
                                                    <dx:DoughnutSeriesLabel Font="Arial, 8pt" MaxLineCount="2" LineVisibility="True" TextAlignment="Near">
                                                        <Shadow Visible="True" />
                                                    </dx:DoughnutSeriesLabel>
                                                </LabelSerializable>
                                                <TopNOptions Enabled="True" />
                                            </dx:Series>
                                        </SeriesSerializable>
                                        <PaletteWrappers>
                                            <dx:PaletteWrapper Name="Palette 1" ScaleMode="Repeat">
                                                <Palette>
                                                    <dx:PaletteEntry Color="255, 255, 0" Color2="183, 205, 36" />
                                                    <dx:PaletteEntry Color="154, 1, 154" Color2="0, 163, 227" />                                                    
                                                    <dx:PaletteEntry Color="194, 23, 23" Color2="100, 84, 158" />
                                                </Palette>
                                            </dx:PaletteWrapper>
                                        </PaletteWrappers>
                                    </dx:WebChartControl>
                                    <%--<dx:WebChartControl ID="WebChartControlDonut" runat="server" CrosshairEnabled="True" Height="345px" Width="798px" 
                                       AppearanceNameSerializable="Light">
                                        <BorderOptions Visibility="False" />
                                        <Legend Name="Status Coves" Visibility="True"></Legend>
                                        <SeriesSerializable>
                                            <dx:Series Name="Status" ArgumentDataMember="STATUS" LabelsVisibility="True"
                                                LegendTextPattern="{A} - {VP:0.00%}"  ToolTipPointPattern="{S}">
                                                <ViewSerializable>
                                                    <dx:DoughnutSeriesView>
                                                    </dx:DoughnutSeriesView>
                                                </ViewSerializable>
                                            </dx:Series>
                                        </SeriesSerializable>
                                        <SeriesTemplate>
                                            <ViewSerializable>
                                                <dx:DoughnutSeriesView>
                                                </dx:DoughnutSeriesView>
                                            </ViewSerializable>
                                        </SeriesTemplate>
                                        <Titles>
                                            <dx:ChartTitle Font="Arial, 14pt" Indent="20" MaxLineCount="10" Text="Status Coves" />
                                        </Titles>
                                    </dx:WebChartControl>--%>
                                    <%--<dx:BootstrapPieChart ID="ChartDonut" runat="server" Type="Doughnut" InnerRadius="0.5" TitleText=" holas" 
                                        SubtitleText="Estatus de Coves" Palette="Soft">
                                        <SettingsToolTip Enabled="true" />
                                        <SeriesCollection>
                                            <dx:BootstrapPieChartSeries >
                                                <Label Visible="true">
                                                    <Format Type="Percent"/>
                                                </Label>
                                            </dx:BootstrapPieChartSeries>
                                        </SeriesCollection>
                                        <SettingsLegend VerticalAlignment="Bottom" HorizontalAlignment="Center" />
                                    </dx:BootstrapPieChart>--%>
                                    <dx:BootstrapPieChart ID="BootstrapPieChart1" runat="server" Type="Doughnut" InnerRadius="0.5" TitleText=" " 
                                        SubtitleText="Estatus de Coves" Palette="Soft" Visible="false">
                                        <SettingsToolTip Enabled="true" OnClientCustomizeTooltip="customizeTooltipDona" Shared="true"/>
                                        <SettingsLegend VerticalAlignment="Bottom" HorizontalAlignment="Center" />                                                                             
                                    </dx:BootstrapPieChart>
                                </div>
                            </div>
                            <%--Gráfica Sin información por el momento...--%>
                            <div class="col-sm-4 col-md-4" style="margin-bottom:20px;height:480px">
                                <div class="bordes_curvos" align="center" style="background-color:#FFFFFF;padding-top:15px;padding-bottom:15px; border-color:#B4EEFC; border-width:1px;border-style:solid;height:100%">
                                    <br /><br /><br /><br /><br /><br /><br />
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Sin información por el momento..." Font-Size="12px" /> 
                                    <%--<dx:BootstrapPieChart ID="BootstrapPieChart2" runat="server" EncodeHtml="True" Type="Doughnut" InnerRadius="0.5" TitleText=" " 
                                        SubtitleText="Estatus de Coves" Palette="Soft" Visible="false" DataSourceID="SqlDataSource1">
                                     <SettingsToolTip Enabled="true" />
                                        <SeriesCollection>
                                            <dx:BootstrapPieChartSeries ArgumentField="STATUS" ValueField="TOTAL">
                                            </dx:BootstrapPieChartSeries>
                                        </SeriesCollection>
                                        <SettingsLegend VerticalAlignment="Bottom" HorizontalAlignment="Center" />  
                                    </dx:BootstrapPieChart>--%>
                                    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD %>" SelectCommand="SICEWEB_INTRO_COVES_GRAFICA" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="DESDE" Name="DESDE" PropertyName="Value" Type="DateTime" />
                                            <asp:ControlParameter ControlID="HASTA" Name="HASTA" PropertyName="Value" Type="DateTime" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>--%>
                                </div>
                            </div>
                        </div>
                     </dx:ContentControl>
                    </ContentCollection>
                 </dx:TabPage>
                 <%--<dx:TabPage Text="Tabla de Marcadores">
                     <ContentCollection>
                     <dx:ContentControl ID="ContentControl2" runat="server">                        
                        <div class="renglon" style="width:100%;height:400px; text-align:center; padding-top:200px">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Sin información por el momento..." Font-Size="12px" />  
                        </div>
                     </dx:ContentControl>
                 </ContentCollection>
                 </dx:TabPage>
                 <dx:TabPage Text="Insights">
                     <ContentCollection>
                     <dx:ContentControl ID="ContentControl3" runat="server">
                        <div class="renglon" style="width:100%;height:400px; text-align:center; padding-top:200px">
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Sin información por el momento..." Font-Size="12px" />  
                        </div>
                     </dx:ContentControl>
                 </ContentCollection>
                 </dx:TabPage>--%>
             </TabPages>
         </dx:ASPxPageControl>
        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
            <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
        </dx:ASPxCallback>
        <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
        </dx:ASPxLoadingPanel>
        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>

        <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
        <div class="modal fade" id="AlertError" tabindex="-1" role="dialog" data-html="true">
            <div class="modal-dialog modal-sm" role="document" style="top: 25%; outline: none;">
                <div class="alert alert-danger text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                    <span class="glyphicon glyphicon-alert ico"></span>
                    <br />
                    <br />
                    <p id="pModalError" runat="server" class="alert-title">
                    </p>                                
                    <hr/>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
        <%--<p class="lead">
            <a class="btn btn-primary btn-lg" href="AcercaDe.aspx" role="button">Acerca de</a>
        </p>--%>
    </div>
</div>
<br /><br />   
</asp:Content>