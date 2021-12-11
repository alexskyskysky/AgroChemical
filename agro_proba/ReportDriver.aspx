<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDriver.aspx.cs" Inherits="agro_proba.ReportDriver" %>

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
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" ShowBackButton="False" ShowDocumentMapButton="False" ShowFindControls="False" ShowZoomControl="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="320mm" Width="240mm" ShowWaitControlCancelLink="False" ViewStateMode="Enabled" PromptAreaCollapsed="True" PageCountMode="Actual" ExportContentDisposition="AlwaysInline">
            <LocalReport ReportEmbeddedResource="agro_proba.DriverReport.rdlc" ReportPath="Reports\DriverReport.rdlc" DisplayName="ReportDriver" EnableExternalImages="True" EnableHyperlinks="True">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="SelectDriverReport" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="agro_proba.ReportsDSTableAdapters.SelectDriverReportTableAdapter" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="id_region" Type="Int32" />
                <asp:Parameter Name="id_worker" Type="Int32" />
                <asp:Parameter Name="id_mission" Type="Int32" />
                <asp:Parameter Name="date_from_plan_long" Type="Int64" />
                <asp:Parameter Name="date_to_plan_long" Type="Int64" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
