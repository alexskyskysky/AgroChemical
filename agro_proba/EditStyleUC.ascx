<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditStyleUC.ascx.cs" Inherits="agro_proba.EditStyleUC" %>

<link type="text/css" href="css/GridPanel.css" rel="stylesheet" />

<script type="text/javascript" src="js/GridPanel.js"></script>
<script type="text/javascript" src="js/EditStyleUC.js"></script>

<asp:Panel ID="EditStyleP" runat="server" Width="100%" Height="100%" style="padding: 0px; margin: 0px;">
    <div id="EditStyleTabs" style="width: inherit; height: inherit; padding: 0px; margin: 0px;">         
        <ul>
            <li><a href="#tab_1">Вкладка 1</a></li>
            <li><a href="#tab_2">Вкладка 2</a></li>
            <li><a href="#tab_3">Вкладка 3</a></li>
        </ul>
        <div id="tab_1" style="width: inherit; height: inherit; padding: 0px; margin: 0px;">
            <div id="EditStyleGP"></div>
        </div>
        <div id="tab_2" style="width: inherit; height: inherit; padding: 0px; margin: 0px;">
            <asp:Label ID="Label2" runat="server" Text="Label 2"></asp:Label>
        </div>
        <div id="tab_3" style="width: inherit; height: inherit; padding: 0px; margin: 0px;">
            <asp:Label ID="Label3" runat="server" Text="Label 3"></asp:Label>
        </div>
    </div>
</asp:Panel>


