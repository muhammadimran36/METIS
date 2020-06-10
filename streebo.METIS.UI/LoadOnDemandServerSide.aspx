<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadOnDemandServerSide.aspx.cs" Inherits="LoadOnDemandServerSide" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	 <style type="text/css">
        span.text
        {
            font: 13px 'Segoe UI' , Arial, sans-serif;
            color: #4888a2;
            padding-right: 10px;
            vertical-align: middle;
            display: inline-block; *display:inline;zoom:1;width:90px;}
        .module-row
        {
            margin: 10px 0;
        }
        .module-row .status-text
        {
            margin-left: 103px;
            display: block;
            font: 13px 'Segoe UI' , Arial, sans-serif;
            color: #4888a2;
        }       
    </style>
	<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
	<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
		<Scripts>
			<%--Needed for JavaScript IntelliSense in VS2010--%>
			<%--For VS2008 replace RadScriptManager with ScriptManager--%>
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
		</Scripts>
	</telerik:RadScriptManager>
	<script type="text/javascript">
		//Put your JavaScript code here.
    </script>
 
	    <div class="module-row">
            <span class="text">Server-Side:</span>
            <telerik:RadComboBox ID="RadComboBox1" runat="server" Width="250px" Height="150px"
                EmptyMessage="Select a Company" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                EnableVirtualScrolling="true" OnItemsRequested="RadComboBox1_ItemsRequested">
            </telerik:RadComboBox>
            <asp:Button runat="server" Text="Select" ID="Button1" OnClick="Button1_Click">
            </asp:Button>
        </div>
        <div class="module-row">
            <asp:Label CssClass="status-text" ID="Label1" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
