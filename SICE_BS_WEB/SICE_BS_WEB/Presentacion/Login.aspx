<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Login" EnableEventValidation="false" %>

<%@ Register assembly="DevExpress.XtraCharts.v17.1.Web, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v17.1, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>      
        body {
            background-color: #FFFFFF;
        }

        .navbar {
            border: none;
            margin-bottom: 10px;
            border-radius: 0 !important;
            border-bottom-left-radius: 6px !important;
            border-bottom-right-radius: 6px !important;
        }
        
        .panel-title {
            font-size: 14px !important;
        }

        .panel-primary {
            border-color: #F8F8F8;
            overflow: hidden;
        }

        .panel-primary > .panel-heading {
            color: #5E5E5E;
            background-color: #E7E7E7;
            border-color: #E7E7E7;
        }
        .btn-primary {
            background-color: #73C000 !important;
            border-color: #73C000 !important;
            color: #fff !important;
        }

        .btn-primary:hover {
            background-color: #62A000 !important;
            border-color: #62A000 !important;
            color: #fff !important;
        }
        
        .btn-primary:active:focus {
            background-color: #62A000 !important;
            border-color: #62A000 !important;
            color: #fff !important;
        }

        .color_invisible{
            color: transparent !important;
        }

        .bordes_curvos{
            border-radius: 0px 5px 5px 0px;
             -moz-border-radius: 5px 5px 5px 5px;
            -khtml-border-radius: 5px 5px 5px 5px;
            -webkit-border-radius: 5px 5px 5px 5px;
            overflow: hidden !important;
            overflow-x: hidden !important;
            overflow-y: hidden !important;                 
        }

        /*Hover del filtro del grid*/
        .dxeListBoxItemHover_SoftOrange {
            background-color: #73C000; /*#ff9460;*/
            color: #FFFFFF;
        }

        /*Botón deshabilitado del filtro del grid*/
        .dxbDisabled_SoftOrange, a.dxbButton_SoftOrange.dxbDisabled_SoftOrange {
            color: #9d9d9d;
            cursor: default;
            text-decoration: none;
        }

        .dxbDisabled_SoftOrange {
            background: #cacaca url(/DXR.axd?r=0_5024-owDhf) repeat-x top;
            border-color: #a2a2a2;
        }

        /*Botón habilitado del filtro del grid*/
        .dxbButton_SoftOrange {
            color: #FFFFFF;
            font: 12px Tahoma, Geneva, sans-serif;
            border: 1px solid #95D732; /*#cb4b31;*/
            background: /*#EF643C*/ #73C000 repeat-x top;
            padding: 1px;
        }

        .dxpcLoadingPanel_SoftOrange, .dxlpLoadingPanel_SoftOrange, .dxlpLoadingPanelWithContent_SoftOrange {
            font: 12px Tahoma, Geneva, sans-serif;
            color: #73C000;
            background-color: White;
            border: 1px solid #73C000;
        }

        .colorgraph {
        height: 7px;
        border-top: 0;
        background: #c4e17f;
        border-radius: 5px;
        background-image: -webkit-linear-gradient(left, #c4e17f, #c4e17f 12.5%, #f7fdca 12.5%, #f7fdca 25%, #fecf71 25%, #fecf71 37.5%, #f0776c 37.5%, #f0776c 50%, #db9dbe 50%, #db9dbe 62.5%, #c49cde 62.5%, #c49cde 75%, #669ae1 75%, #669ae1 87.5%, #62c2e4 87.5%, #62c2e4);
        background-image: -moz-linear-gradient(left, #c4e17f, #c4e17f 12.5%, #f7fdca 12.5%, #f7fdca 25%, #fecf71 25%, #fecf71 37.5%, #f0776c 37.5%, #f0776c 50%, #db9dbe 50%, #db9dbe 62.5%, #c49cde 62.5%, #c49cde 75%, #669ae1 75%, #669ae1 87.5%, #62c2e4 87.5%, #62c2e4);
        background-image: -o-linear-gradient(left, #c4e17f, #c4e17f 12.5%, #f7fdca 12.5%, #f7fdca 25%, #fecf71 25%, #fecf71 37.5%, #f0776c 37.5%, #f0776c 50%, #db9dbe 50%, #db9dbe 62.5%, #c49cde 62.5%, #c49cde 75%, #669ae1 75%, #669ae1 87.5%, #62c2e4 87.5%, #62c2e4);
        background-image: linear-gradient(to right, #c4e17f, #c4e17f 12.5%, #f7fdca 12.5%, #f7fdca 25%, #fecf71 25%, #fecf71 37.5%, #f0776c 37.5%, #f0776c 50%, #db9dbe 50%, #db9dbe 62.5%, #c49cde 62.5%, #c49cde 75%, #669ae1 75%, #669ae1 87.5%, #62c2e4 87.5%, #62c2e4);
        }
        .navbar-brand {
        padding: 0 !important;
        margin: 0 !important;
        /*line-height: 50px;*/
}

    .navbar-brand > img {
        height: 50px;
    }

    .bordes_curvos_derecha{
            border-radius: 0px 5px 5px 0px;
             -moz-border-radius: 0px 5px 5px 0px;
            -khtml-border-radius: 0px 5px 5px 0px;
            -webkit-border-radius: 0px 5px 5px 0px;
            overflow: hidden !important;
            overflow-x: hidden !important;
            overflow-y: hidden !important;         
    }

    .tool {
        font-size:12px;
    }

    .tooltip {
  position: absolute;
  z-index: 1070;
  display: block;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  font-size: 12px;
  font-style: normal;
  font-weight: normal;
  line-height: 1.42857143;
  text-align: left;
  text-align: start;
  text-decoration: none;
  text-shadow: none;
  text-transform: none;
  letter-spacing: normal;
  word-break: normal;
  word-spacing: normal;
  word-wrap: normal;
  white-space: normal;
  filter: alpha(opacity=0);
  opacity: 0;

  line-break: auto;
}
.tooltip.in {
  filter: alpha(opacity=90);
  opacity: .9;
}
.tooltip.top {
  padding: 5px 0;
  margin-top: -3px;
}
.tooltip.right {
  padding: 0 5px;
  margin-left: 3px;
}
.tooltip.bottom {
  padding: 5px 0;
  margin-top: 3px;
}
.tooltip.left {
  padding: 0 5px;
  margin-left: -3px;
}
.tooltip-inner {
  max-width: 200px;
  padding: 3px 8px;
  color: #fff;
  text-align: center;
  background-color: #000;
  border-radius: 4px;
}
.tooltip-arrow {
  position: absolute;
  width: 0;
  height: 0;
  border-color: transparent;
  border-style: solid;
}
.tooltip.top .tooltip-arrow {
  bottom: 0;
  left: 50%;
  margin-left: -5px;
  border-width: 5px 5px 0;
  border-top-color: #000;
}
.tooltip.top-left .tooltip-arrow {
  right: 5px;
  bottom: 0;
  margin-bottom: -5px;
  border-width: 5px 5px 0;
  border-top-color: #000;
}
.tooltip.top-right .tooltip-arrow {
  bottom: 0;
  left: 5px;
  margin-bottom: -5px;
  border-width: 5px 5px 0;
  border-top-color: #000;
}
.tooltip.right .tooltip-arrow {
  top: 50%;
  left: 0;
  margin-top: -5px;
  border-width: 5px 5px 5px 0;
  border-right-color: #000;
}
.tooltip.left .tooltip-arrow {
  top: 50%;
  right: 0;
  margin-top: -5px;
  border-width: 5px 0 5px 5px;
  border-left-color: #000;
}
.tooltip.bottom .tooltip-arrow {
  top: 0;
  left: 50%;
  margin-left: -5px;
  border-width: 0 5px 5px;
  border-bottom-color: #000;
}
.tooltip.bottom-left .tooltip-arrow {
  top: 0;
  right: 5px;
  margin-top: -5px;
  border-width: 0 5px 5px;
  border-bottom-color: #000;
}
.tooltip.bottom-right .tooltip-arrow {
  top: 0;
  left: 5px;
  margin-top: -5px;
  border-width: 0 5px 5px;
  border-bottom-color: #000;
}

    </style>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            //Inicializa todos los tooltip
            $(function () {
                $('[data-toggle="popover"]').popover();
                $('[data-toggle="tooltip"]').tooltip({
                    delay: 500,
                    trigger: 'hover'
                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <!-- Generated markup by the plugin -->
    <div class="tooltip top" role="tooltip">
        <div class="tooltip-arrow"></div>
        <div class="tooltip-inner"></div>
    </div>
    <div class="container">
        <br/>
        <div class="col-lg-6 col-lg-offset-3 col-sm-6 col-sm-offset-3 col-xs-10 col-xs-offset-1 paddingLeftLogin">
            <div class="panel panel-default login">
                <div class="panel-heading text-center">
                    <%--<img src="../img/logo.jpg" />--%>
                    <h4 class="form-signin-heading1">Bienvenido a SICE VUCEM</h4>
                    <hr class="colorgraph" />
                </div>
                <div class="panel-body" style="background-color: lightgray;">
                    <asp:Panel ID="PanelBody" runat="server" DefaultButton="btnInicio">
                        <br />
                        <div class="form-group input-group textWidth" style="position: relative; float: left;" title="Usuario" data-toggle="tooltip">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                            <asp:TextBox ID="txtUsuario" runat="server" type="text" class="form-control" placeholder="Usuario" MaxLength="30" BackColor="#F8F8F8">
                            </asp:TextBox>
                        </div>
                        <div class="form-group input-group textWidth" style="position: relative; float: left;" title="Contraseña" data-toggle="tooltip">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-eye-close"></i></span>
                            <asp:TextBox ID="txtPassword" runat="server" type="password" class="form-control" TextMode="Password" BackColor="#F8F8F8" placeholder="Contraseña" MaxLength="50"></asp:TextBox>
                        </div>

                        <%--<div class="form-group input-group textWidth" style="position: relative; float: left;" title="Base De Datos" data-toggle="tooltip">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-hdd"></i></span>
                            <asp:DropDownList ID="ddlBase" runat="server" class="form-control" BackColor="#F8F8F8" placeholder="Base de Datos">
                                <asp:ListItem Text="Bebidas" Value="1"></asp:ListItem>
                                <asp:ListItem Text="IEBM" Value="2"></asp:ListItem>
                                <asp:ListItem Text="MAPSA" Value="3" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                        <%--<div class="form-group input-group textWidth">
                            <span class="input-group-addon" id="basic-addon6">
                                <span class="glyphicon glyphicon-hdd" aria-hidden="true"></span>
                            </span>
                            <dx:ASPxComboBox ID="cbBase" runat="server" Height="30px" NullText="Base de Datos" DataSecurityMode="Default"
                                Width="100%" Font-Size="14px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" ForeColor="#6B5555">
                                <Items>
                                    <dx:ListEditItem Text="Bebidas" Value="1" />
                                    <dx:ListEditItem Text="Embotelladora" Value="2" />
                                    <dx:ListEditItem Text="Manantial" Value="3" Selected="true" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>--%>
                        <div class="text-center" style="padding-top: 1%; padding-bottom: 1%">
                            <%--<asp:Button ID="LinkButton1" OnClick="LinkButton1_Click" runat="server" Text="Iniciar Sesión" class="btn btn-primary btn-md btn-block" />--%>
                            <dx:BootstrapButton ID="btnInicio" runat="server" Text="Iniciar Sesión" OnClick="btnInicio_Click" 
                                 SettingsBootstrap-RenderOption="Primary" CssClasses-Control="btn btn-primary btn-md btn-block" AutoPostBack="false" >
                                 <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                   Init="function(s, e) {LoadingPanel1.Hide(); }" />
                             </dx:BootstrapButton>
                        </div>
                        <div class="text-center">
                            <h6>v<%# version %></h6>
                        </div>
                    </asp:Panel>
                    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                     <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                    </dx:ASPxCallback>
                    <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                        Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                    </dx:ASPxLoadingPanel>
                    <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                </div>
            </div>
        </div>        
    </div>
    <button id="btnModalBD" type="button" data-toggle="modal" data-target="#ModalBD" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalBD" tabindex="-1" role="dialog" aria-labelledby="ModalBDTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalBDTitulo" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div runat="server" id="Div1">
                                    <dx:ASPxLabel ID="lb_S" runat="server" Text="Seleccione una Base de Datos:" Font-Size="13px" />
                                </div>
                                <div runat="server" id="Div2">&nbsp;</div>                                                                  
                                <div runat="server" id="Div3">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" title="Bases de Datos" data-toggle="tooltip">
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon1">
                                                <span class="glyphicon glyphicon-hdd" aria-hidden="true"></span>
                                            </span>
                                            <dx:ASPxComboBox ID="cmbBD" Caption="" runat="server" Height="30px" NullText="* Base de Datos" DataSecurityMode="Default" Width="100%" Font-Size="12px" 
                                                Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555" TextField="TextoBD" ValueField="ValorBD">                                                
                                                <ClearButton DisplayMode="Never" />
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group text-right" style="position: relative; width: 100%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
                                <br /><br />
                                <br /><br />
                                <br /><br />
                                <br /><br />                                
                            </div>    
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <dx:BootstrapButton ID="btnIniciar" runat="server" Text="Iniciar Sesión" OnClick="btnIniciar_Click" 
                                 SettingsBootstrap-RenderOption="Success" CssClasses-Control="btn btn-success btn-sm" AutoPostBack="false"
                                CssClasses-Icon="glyphicon glyphicon-ok" >                                 
                                 <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                   Init="function(s, e) {LoadingPanel1.Hide(); }" />
                             </dx:BootstrapButton>

                            <%--<asp:LinkButton ID="btnIniciar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar" OnClick="btnIniciar_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Iniciar Sesión
                            </asp:LinkButton>--%>
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">
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
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
