<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="agro_proba.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1" Width="300" runat="server" />
        <br />
        <asp:Button ID="UploadB" runat="server" Text="Загрузить и обработать" OnClick="UploadB_Click" />
        <br />
        <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
