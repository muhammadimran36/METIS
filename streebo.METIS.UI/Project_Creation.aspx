<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Project_Creation.aspx.cs" Inherits="streebo.METIS.UI.Project_Creation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <asp:TextBox ID="ProjectName" runat="server"></asp:TextBox>
        
        <asp:DropDownList ID="ProjectType" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
        </asp:DropDownList>
        
        <asp:Button ID="btnSubmit" runat="server" onclick="Button1_Click" 
            Text="Submit" />
        <br />
        <asp:Label ID="lblError" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Label ID="lblStartTime" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Label ID="lblEndTime" runat="server" Text="Label"></asp:Label>
    </div>
 
    </form>
</body>
</html>
