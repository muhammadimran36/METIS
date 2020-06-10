<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Practices.aspx.cs" Inherits="streebo.METIS.UI.Practices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DropDownList ID="ddlDepartment" runat="server" AppendDataBoundItems="true" 
            AutoPostBack="True" 
          Visible="true" 
            Width="175px">
            <Items>
                <asp:ListItem Text="All" />
                <asp:ListItem>Information Management</asp:ListItem>
                <asp:ListItem>SSL-Portal</asp:ListItem>
            </Items>
        </asp:DropDownList>
    
    </div>
    </form>
</body>
</html>
