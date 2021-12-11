<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportSignificativeBySoilMoscowYear.aspx.cs" Inherits="agro_proba.ReportSignificativeBySoilMoscowYear" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" ShowBackButton="False" ShowDocumentMapButton="False" ShowFindControls="False" ShowZoomControl="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="220mm" Width="310mm" ShowWaitControlCancelLink="False" ViewStateMode="Enabled" PromptAreaCollapsed="True" PageCountMode="Actual" ExportContentDisposition="AlwaysInline">
            <LocalReport ReportEmbeddedResource="agro_proba.SignificativeBySoilMoscowYear.rdlc" ReportPath="Reports\SignificativeBySoilMoscowYear.rdlc" DisplayName="GroupsByTypeFarmlandMoscowYear" EnableExternalImages="True" EnableHyperlinks="True">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="SelectSignificativeBySoilFromMoscowYear" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="agro_proba.ReportsDSTableAdapters.SelectSignificativeBySoilFromMoscowYearTableAdapter" />
    </form>
</body>
</html>
