﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Menu"></asp:Label>
        <asp:DropDownList ID="ddlMenu" runat="server">
        </asp:DropDownList>
        <asp:Label ID="Label2" runat="server" Text="Qty"></asp:Label>
        <asp:TextBox ID="txtQty" runat="server"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="Add" />
        <br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <asp:HiddenField ID="Orders" Value='[]' runat="server" />
    </div>
    
    </form>
</body>
</html>
