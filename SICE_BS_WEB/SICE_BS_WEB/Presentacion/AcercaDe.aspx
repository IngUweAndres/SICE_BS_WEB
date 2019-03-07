<%@ Page Title="Acerca de" Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="AcercaDe.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.AcercaDe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    
    <div class="background-acercade"></div>
    <div class="container acercade">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Acerca de SICE VUCEM</h3>
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <div class="row">
                        <div class="col-6 col-md-6 text-center">
                            <div class="inaction-logo">
                                <img src="../img/logo.jpg" />
                            </div>
                            <br />
                            <asp:LinkButton ID="btnPdf" runat="server" CssClass="btn btn-primary btn-sm" ToolTip="Manual SICE-VUCEM WEB"
                                 OnClientClick="" OnClick="btnPdf_Click">
                                 <span class="glyphicon glyphicon-save-file"></span>&nbsp;&nbsp;Manual SICE-VUCEM WEB
                            </asp:LinkButton>
                            <br /><br />
                        </div>
                        <div class="col-6 col-md-6">
                            <h4>SICE VUCEM System Web</h4>
                            <p>Sistema de informacion de comercio exterior, es un sistema automatizado que te permite estar en linea con la aduana mexicana.</p>
                            <p>
                                Desarrollado por: Sac Grupo de Ingeniería S.A. de C.V
                            </p>
                            <p>Cracovia #72, Edif. B, BPO-05, San Ángel, C.P. 01000, Ciudad de México, México</p>
                            <p>Tel 01(55) 55439959 / 01(55) 55171144</p>  
                        </div>
                        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                    </div>                    
                </div>
            </div>
        </div>
    </div>
    <br /><br />
    <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
    <div class="modal fade" id="AlertError" tabindex="-1" role="dialog">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
